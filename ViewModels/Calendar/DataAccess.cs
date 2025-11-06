using Microsoft.Data.Sqlite;
using PlanningProgramV3.Models;
using PlanningProgramV3.ViewModels.ItemViewModels;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace PlanningProgramV3.ViewModels.Calendar
{
    //https://learn.microsoft.com/en-us/windows/apps/develop/data-access/sqlite-data-access
    public static class DataAccess
    {
        #region Final Data Base Related Stuff
        public static event Action InitializeTable;

        //Use connection pooling for databases
            //cursor vs prepared statemetn. 
                //cursor puts a command on the server, and say it's going to return a million rows, execute on the server, only get 10 rows at a time
                //prepared statement send statement, generates query plans, does 2 things: security, avoids sql injection attack, other thing, is performance boost, rerun command
            //create command is good.


        /// <summary>
        /// This is the final method I will use to create or access the database for calendar tasks
        /// </summary>
        public static async void AccessOrCreateDatabase()
        {
            using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
            {
                connection.Open();

                //create or access the tables via command
                //For constraints/triggers:
                    //maybe don't delete plans or plans from tables - checking tables to see if data is no longer referenced will be harder than just leaving it alone
                    //can you update primary keys? Is that allowed?
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS VersionInfo(
                        VersionID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Major TINYINT UNSIGNED,
                        Minor TINYINT UNSIGNED,
                        Revision TINYINT UNSIGNED,
                        Build TINYINT UNSIGNED,
                        CONSTRAINT UC_UniqueVals UNIQUE (Major,Minor,Revision,Build)
                    );
                    
                    CREATE TABLE IF NOT EXISTS Plan(
                        PlanID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FilePath TEXT NOT NULL,
                        PlanName VARCHAR(50) NOT NULL,
                        VersionID UNSIGNED NOT NULL,
                        FOREIGN KEY (VersionID) REFERENCES VersionInfo(VersionID),
                        UNIQUE(FilePath)
                    );

                    CREATE TABLE IF NOT EXISTS Task(
                        TaskGUID VARCHAR(36) PRIMARY KEY,
                        TaskName VARCHAR(50) NOT NULL,
                        Completion bool, 
                        XWorldCoord INTEGER NOT NULL,
                        YWorldCoord INTEGER NOT NULL,
                        PlanID INT UNSIGNED NOT NULL,
                        FOREIGN KEY (PlanID) REFERENCES Plan(PlanID)
                    );

                    CREATE TABLE IF NOT EXISTS TaskDate(
                        TaskDateID INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartDate datetime NOT NULL,
                        EndDate datetime NOT NULL,
                        TaskGUID VARCHAR(36) NOT NULL,
                        FOREIGN KEY (TaskGUID) REFERENCES Task(TaskGUID),
                        CONSTRAINT UC_UniqueStartEndTask UNIQUE (TaskGUID,StartDate,EndDate)
                    );
                ";

                //unique queries are present for values that should be unique, but that may not be stable - hence why they're not used as primary keys

                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
        }


        /// <summary>
        /// This method will add a taskdate to the database
        /// </summary>
        /// <param name="plan">the plan which the task comes from</param>
        /// <param name="task">the task to add</param>
        /// <param name="TaskDateData">the corresponding dates to add</param>
        public static async void AsyncAddTaskDateToDatabase(PlannerViewModel plan,TaskViewModel task, DateDurationViewModel TaskDateData)
        {
            //create connection, and just pass in to all methods
            using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
            {
                connection.Open();
                //for right now, assume the version data is accurate
                long planID = GetIDOrAddPlan(plan, connection);
                //up next, get/add the task. This will, by far, be the second easiest one to do
                    //since task ids are just the guids, and those can't change, I need only INSERT OR IGNORE the task
                    //in fact, I could just add the date in one fell swoop with this

                var addTaskAndDateCommand = connection.CreateCommand();
                //do as insert or ignore to ensure someone doesn't add two of the same tasks for the same date
                addTaskAndDateCommand.CommandText =
                    @" INSERT OR IGNORE INTO Task (TaskGUID, TaskName, Completion, XWorldCoord, YWorldCoord, PlanID)
                       VALUES(@taskGUID,@taskName,@completion,@xWorldCoord,@yWorldCoord,@planID);
                       INSERT OR IGNORE INTO TaskDate (StartDate, EndDate, TaskGUID)
                       VALUES(@startDate,@endDate,@taskGUID);
                    ";
                addTaskAndDateCommand.Parameters.AddWithValue("@taskGUID", task.UUID);
                addTaskAndDateCommand.Parameters.AddWithValue("@taskName", task.Name);
                addTaskAndDateCommand.Parameters.AddWithValue("@completion", task.IsComplete);
                addTaskAndDateCommand.Parameters.AddWithValue("@xWorldCoord", task.X);
                addTaskAndDateCommand.Parameters.AddWithValue("@yWorldCoord", task.Y);
                addTaskAndDateCommand.Parameters.AddWithValue("@planID", planID);
                addTaskAndDateCommand.Parameters.AddWithValue("@startDate", TaskDateData.StartDate);
                addTaskAndDateCommand.Parameters.AddWithValue("@endDate", TaskDateData.EndDate);
                
                await addTaskAndDateCommand.ExecuteNonQueryAsync();
                //now that the database is updated, close the connection
                connection.Close();
            }
        }

        /// <summary>
        /// This method will get the id of a version data in the VersionInfo table
        ///     if it does not already exist, then it will be added
        /// should only ever be called in methods where a connection already exists
        /// </summary>
        /// <param name="versionData">The versionData to be passed in</param>
        /// <param name="connection">the connection to the database</param>
        /// <returns>the id of the versionInfo added</returns>
        /// <exception cref="Exception">Thrown if no version data is found even after the version is inserted</exception>
        private static long GetOrAddVersion(VersionData versionData,SqliteConnection connection)
        {
            var getVersionID = connection.CreateCommand();
            getVersionID.CommandText =
                @"  SELECT VersionID FROM VersionInfo
                    WHERE Major=@major AND Minor=@minor AND Revision=@revision AND Build=@build;
                ";
            getVersionID.Parameters.AddWithValue("@major", versionData.major);
            getVersionID.Parameters.AddWithValue("@minor", versionData.minor);
            getVersionID.Parameters.AddWithValue("@revision", versionData.revision);
            getVersionID.Parameters.AddWithValue("@build", versionData.build);
            SqliteDataReader query = getVersionID.ExecuteReader();
            if (query.Read())
            {
                return (long)query["VersionID"];
            }

            //if the value was not returned, then add it, and re-get the data
            var addVersionID = connection.CreateCommand();
            addVersionID.CommandText =
                @" INSERT INTO VersionInfo (Major, Minor, Revision, Build)
                   Values(@major,@minor,@revision,@build);
                   
                   SELECT VersionID FROM VersionInfo
                   WHERE Major=@major AND Minor=@minor AND Revision=@revision AND Build=@build;
                ";

            addVersionID.Parameters.AddWithValue("@major", versionData.major);
            addVersionID.Parameters.AddWithValue("@minor", versionData.minor);
            addVersionID.Parameters.AddWithValue("@revision", versionData.revision);
            addVersionID.Parameters.AddWithValue("@build", versionData.build);
            SqliteDataReader query2 = addVersionID.ExecuteReader();
            if (query2.HasRows && query2.Read())
            {
                //will have id, and so return the data
                return (long)query2["VersionID"];
            }
            else
            {
                throw new Exception("The version info was added, but was not found in the version info table");
                return -1;
            }
        }

        /// <summary>
        /// This method will be used to get the id in the table 
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="connection"></param>
        /// <returns>the id of the plan that was found or added</returns>
        /// <exception cref="Exception">Thrown if no plan is found, even after it's inserted</exception>
        private static long GetIDOrAddPlan(PlannerViewModel plan, SqliteConnection connection)
        {
            long planID = -1;
            var getPlanCommand = connection.CreateCommand();
            //right now, easier solution for me than to add the ids to the date duration data, is for me to simply query the filepath
            //filepath should stay accurate - even if there's no file there, it must be unique, and it's easy to get without a back and forth of data to the plan
            getPlanCommand.CommandText =
                @"SELECT PlanID FROM Plan
                      WHERE FilePath=@FilePath;
                    ";

            getPlanCommand.Parameters.AddWithValue("@FilePath", plan.FilePath);

            //making it asynchronous to not tank frame rate, though will thread be blocked? If not, how know when done

            SqliteDataReader query = getPlanCommand.ExecuteReader();
            //check if a result was found by that value, and if not, throw error
            if (query.HasRows && query.Read())
            {
                //assume version is most updated
                //planID = query.GetInt64(0);
                
                planID = (long)query["PlanID"]; 
                return planID;
            }
            //otherwise
            //select relevant versionID from the table, or add it
            long versionID = GetOrAddVersion(plan.version, connection);
            var addPlan = connection.CreateCommand();
            addPlan.CommandText =
                @"
                    INSERT INTO Plan (FilePath,PlanName,VersionID)
                    Values(@filePath,@fileName,@versionID);
                    SELECT PlanID FROM Plan
                      WHERE FilePath=@filePath;
                ";

            addPlan.Parameters.AddWithValue("@filePath", plan.FilePath);
            addPlan.Parameters.AddWithValue("@fileName", plan.FileName);
            addPlan.Parameters.AddWithValue("@versionID", versionID);

            //addPlan.ExecuteNonQuery();

            //can rerun the getPlanCommand as the filepath can't have changed
            var queryGet = addPlan.ExecuteReader();
            if (queryGet.HasRows && queryGet.Read())
            {
                planID = (long)queryGet["PlanID"];
                return planID;
            }
            else
            {
                throw new Exception("Plan was added to the table, but wasn't gotten by select statement");
                return -1;
            }

            //add the plan to the file, assume updating it can come later


        }

        //for getting list of calendar tasks
        /* |==================================================================================================================|
         * |Variables and info: TPS = Time Period Start | TPE = Time Period End | TSD = Task Start Date | TSE = Task End Date |
         * |         Assume Task End Date >= Task Start Date                                                                  |
         * |Case 1: Task starts in and ends in time period:                                                                   |
         * |         Task Start Date >= Time Period Start && Task Start Date <= Time Period End                               |
         * |         Task End Date   >= Time Period Start && Task End Date   <= Time Period End                               |
         * |                                                                                                                  |
         * |Case 2: Task starts in time period, and ends after time period                                                    |
         * |         Task Start Date >= Time Period Start && Task Start Date <= Time Period End                               |
         * |         Task End Date   >= Time Period Start && Task End Date   >= Time Period End                               |
         * |                                                                                                                  |
         * |Case 3: Task starts before time period, and ends after time period                                                |
         * |         Task Start Date < Time Period Start && Task Start Date <= Time Period End                                |
         * |         Task End Date  >= Time Period Start && Task End Date   <= Time Period End                                |
         * |                                                                                                                  |
         * |Case 4: Task starts before and ends after time period                                                             |
         * |         Task Start Date < Time Period Start && Task Start Date <= Time Period End                                |
         * |         Task End Date  >= Time Period Start && Task End Date   >= Time Period End                                |
         * |                                                                                                                  |
         * |                                                                                                                  |
         * |                                                                                                                  |
         * |Commonallities between all:                                                                                       |
         * |     Task Start Date MUST be before Time Period End                                                               |
         * |     Task End Date   MUST be after Time Period Start                                                              |
         * |==================================================================================================================|
         *      
         */

        /// <summary>
        /// Gets the tasks from a specific day and none others
        /// </summary>
        /// <param name="date">the date which all tasks found should occur within</param>
        /// <returns name="returnList">A lisst of CalendarTaskData objects returned by the list</returns>
        public static List<CalendarTaskData>? GetTasksFromDate(DateTime date)
        {
            List<CalendarTaskData> returnList = new List<CalendarTaskData>();
            using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
            {
                connection.Open();
                var accessCommand = connection.CreateCommand();
                accessCommand.CommandText =
                    @"SELECT t.TaskGUID, t.TaskName, t.TaskCompletion, td.StartDate, td.EndDate
                      FROM TaskDate td
                      LEFT JOIN Task t ON td.TaskGUID = t.TaskGUID
                      WHERE td.StartDate<=@date AND td.EndDate>=@date;
                    ";
                accessCommand.Parameters.AddWithValue("@date", date);
                SqliteDataReader query = accessCommand.ExecuteReader();
                if (!query.HasRows)
                    return returnList;
                //ensure that there's at least one array in there
                object[] temp;
                while (query.Read())
                {
                    temp = new object[5];
                    query.GetValues(temp);
                    returnList.Add(new CalendarTaskData(temp));
                }

                connection.Close();
            }
            return returnList;
        }

        /// <summary>
        /// Returns all TaskDate objects that occur within a time period
        /// </summary>
        /// <param name="dateStart">the start of the time period to check</param>
        /// <param name="dateEnd">the end of the time period to check</param>
        /// <returns></returns>
        public static List<CalendarTaskData>? GetTasksFromTimePeriod(DateTime dateStart, DateTime dateEnd)
        {
            List<CalendarTaskData> returnList = new List<CalendarTaskData>();
            using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
            {
                connection.Open();
                var accessCommand = connection.CreateCommand();
                accessCommand.CommandText =
                    @"SELECT t.TaskGUID, t.TaskName, t.Completion, td.StartDate, td.EndDate
                      FROM TaskDate td
                      LEFT JOIN Task t ON td.TaskGUID = t.TaskGUID
                      WHERE td.StartDate<=@dateEnd AND td.EndDate>=@dateStart;
                    ";
                accessCommand.Parameters.AddWithValue("@dateStart", dateStart);
                accessCommand.Parameters.AddWithValue("@dateEnd", dateEnd);
                SqliteDataReader query = accessCommand.ExecuteReader();
                if (!query.HasRows)
                    return returnList;
                //ensure that there's at least one array in there
                object[] temp;
                while (query.Read())
                {
                    temp = new object[5];
                    query.GetValues(temp);
                    returnList.Add(new CalendarTaskData(temp));
                }

                connection.Close();
            }
            return returnList;
        }

        #endregion


        //#region Test Database Stuff
        ///// <summary>
        ///// This is the method I will be using for testing purposes for accessing the database. 
        ///// </summary>
        //public static void InitializeDatabase()
        //{
        //    //how partitition data by year???

        //    #region For testing purposes only - delete in final -- how to make work in final only????


        //    #endregion
        //    using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
        //    {
        //        connection.Open();

        //        //so calendar calendarTasks need a few columns: date start, date end, task name, completion, task id, filename????

        //        var command = connection.CreateCommand();
        //        //thinking here: maybe I want a table for tasks that have date durations, and then a table for the actual start and end dates themselves - that way I'm not saving unnecessary data
        //        command.CommandText =
        //            @"

                        
        //                CREATE TABLE IF NOT EXISTS CalendarTasks(
        //                    tableGUID VARCHAR(36) PRIMARY KEY,
        //                    taskGUID VARCHAR(36),
        //                    TaskFileName TEXT,
        //                    TaskName TEXT NOT NULL,
        //                    TaskCompletion BOOLEAN NOT NULL,
        //                    DateStart DATE NOT NULL,
        //                    DateEnd DATE NOT NULL
        //            );
        //            INSERT OR REPLACE INTO CalendarTasks VALUES ('TABLEGUID1','GUID1', 'TaskFileName', 'TaskName1', 1, '2025-08-26 00:00:00.000','2025-08-27 00:00:00.000'),
        //                                                        ('TABLEGUID2','GUID2', 'TaskFileName', 'TaskName2', 0, '2025-08-28 00:00:00.000','2025-08-31 00:00:00.000'),
        //                                                        ('TABLEGUID3','GUID3', 'TaskFileName', 'TaskName3', 0, '2025-08-27 00:00:00.000', '2025-09-10 00:00:00.000');";
        //        //FURTHER TEST STUFF
        //        //('GUID2', 'TaskFileName', 'TaskName2', 0, '2025-08-28 00:00:00.000','2025-08-31 00:00:00.000'),
        //        //('GUID3', 'TaskFileName', 'TaskName3', 0, '2025-08-27 00:00:00.000', '2025-09-01 00:00:00.000')
        //        //System.Console.WriteLine(connection.ConnectionString);
        //        command.ExecuteNonQuery();
        //    }
        //}


        //#region Methods for getting the Data
        ////TABLE FOR TELLING IF A TASK IS WITHIN A PERIOD OF TIME;
        ////ASSUME THAT THE TASK'S END DATE IS MORE THAN OR EQUAL TO THE TASK START DATE
        ////BOTH START AND END DATES ARE INCLUSIVE
        ///* |==================================================================================================================|
        // * |Variables and info: TPS = Time Period Start | TPE = Time Period End | TSD = Task Start Date | TSE = Task End Date |
        // * |         Assume Task End Date >= Task Start Date                                                                  |
        // * |Case 1: Task starts in and ends in time period:                                                                   |
        // * |         Task Start Date >= Time Period Start && Task Start Date <= Time Period End                               |
        // * |         Task End Date   >= Time Period Start && Task End Date   <= Time Period End                               |
        // * |                                                                                                                  |
        // * |Case 2: Task starts in time period, and ends after time period                                                    |
        // * |         Task Start Date >= Time Period Start && Task Start Date <= Time Period End                               |
        // * |         Task End Date   >= Time Period Start && Task End Date   >= Time Period End                               |
        // * |                                                                                                                  |
        // * |Case 3: Task starts before time period, and ends after time period                                                |
        // * |         Task Start Date < Time Period Start && Task Start Date <= Time Period End                                |
        // * |         Task End Date  >= Time Period Start && Task End Date   <= Time Period End                                |
        // * |                                                                                                                  |
        // * |Case 4: Task starts before and ends after time period                                                             |
        // * |         Task Start Date < Time Period Start && Task Start Date <= Time Period End                                |
        // * |         Task End Date  >= Time Period Start && Task End Date   >= Time Period End                                |
        // * |                                                                                                                  |
        // * |                                                                                                                  |
        // * |                                                                                                                  |
        // * |Commonallities between all:                                                                                       |
        // * |     Task Start Date MUST be before Time Period End                                                               |
        // * |     Task End Date   MUST NOT be before Time Period Start                                                         |
        // * |==================================================================================================================|
        // *      
        // */

        //public static List<CalendarTaskData> GetTasksFromWeek(DateTime WeekStart, DateTime WeekEnd)
        //{
        //    var entries = new List<CalendarTaskData>();
        //    #region ONLY FOR TESTING PURPOSES- WILL CHANGE IN FINAL VERSION - HOW DO I DO THIS?
        //    using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
        //    {
        //        connection.Open();
        //        var selectCommmand = new SqliteCommand(
        //            "SELECT * FROM CalendarTasks WHERE DateStart<=@WeekEnd AND DateEnd>=@WeekStart;", connection);
        //        //Only getting id, task name, task completion, date start, and date end because those are the ones which the calendars NEED to display
        //        //the guid, and task file name aren't necessarily necessary, and so storing them would be a waste- can instead fetch them from the table
        //        //but maybe can store bool as to whether or not those are empty?
        //        selectCommmand.Parameters.AddWithValue("@WeekStart", WeekStart);
        //        selectCommmand.Parameters.AddWithValue("@WeekEnd", WeekEnd);

        //        SqliteDataReader query = selectCommmand.ExecuteReader();
        //        //var db = new DataContext(connection);
        //        while (query.Read())
        //        {

        //            //how turn data from row to this -- might be my solution? https://learn.microsoft.com/en-us/dotnet/api/system.data.linq.datacontext.executequery?view=netframework-4.8.1&redirectedfrom=MSDN#System_Data_Linq_DataContext_ExecuteQuery__1_System_String_System_Object___

                    
        //            var temp = new CalendarTaskData(
        //                query.GetString(0),
        //                //query.GetString(1) != null ? true : false,
        //                //query.GetString(2) != null ? true : false,
        //                query.GetString(3),
        //                query.GetBoolean(4),
        //                query.GetDateTime(5),
        //                query.GetDateTime(6));
        //            entries.Add(temp);
        //        }
        //    }
        //    #endregion

        //    return entries;
        //}

        ////LITERALLY FOR THE LOVE OF G-D, DO NOT USE THIS OUTSIDE OF TESTING PURPOSES!!!
        //public static List<CalendarTaskData> GetAllTasks()
        //{
        //    var entries = new List<CalendarTaskData>();
        //    #region ONLY FOR TESTING PURPOSES- WILL CHANGE IN FINAL VERSION - HOW DO I DO THIS?
        //    using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
        //    {
        //        connection.Open();
        //        var selectCommmand = new SqliteCommand(
        //            "SELECT * FROM CalendarTasks WHERE 1;", connection);
        //        //Only getting id, task name, task completion, date start, and date end because those are the ones which the calendars NEED to display
        //        //the guid, and task file name aren't necessarily necessary, and so storing them would be a waste- can instead fetch them from the table when requested
        //        //but maybe can store bool as to whether or not those are empty?

        //        SqliteDataReader query = selectCommmand.ExecuteReader();
        //        //var db = new DataContext(connection);
        //        while (query.Read())
        //        {

        //            //how turn data from row to this -- might be my solution? https://learn.microsoft.com/en-us/dotnet/api/system.data.linq.datacontext.executequery?view=netframework-4.8.1&redirectedfrom=MSDN#System_Data_Linq_DataContext_ExecuteQuery__1_System_String_System_Object___


        //            var temp = new CalendarTaskData(
        //                query.GetString(0),
        //                //query.GetString(1) != null ? true : false,
        //                //query.GetString(2) != null ? true : false,
        //                query.GetString(3),
        //                query.GetBoolean(4),
        //                query.GetDateTime(5),
        //                query.GetDateTime(6));
        //            entries.Add(temp);
        //        }
        //    }
        //    #endregion

        //    return entries;
        //}

        //public static List<CalendarTaskData> GetTasksFromMonth(DateTime MonthStart, DateTime MonthEnd)
        //{
        //    var entries = new List<CalendarTaskData>();
        //    #region ONLY FOR TESTING PURPOSES- WILL CHANGE IN FINAL VERSION - HOW DO I DO THIS?
        //    using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
        //    {
        //        connection.Open();
        //        var selectCommmand = new SqliteCommand(
        //            "SELECT * FROM CalendarTasks WHERE DateStart<=@MonthEnd AND DateEnd>=@MonthStart", connection);
        //        //Only getting id, task name, task completion, date start, and date end because those are the ones which the calendars NEED to display
        //        //the guid, and task file name aren't necessarily necessary, and so storing them would be a waste- can instead fetch them from the table
        //        //but maybe can store bool as to whether or not those are empty?
                    

        //        selectCommmand.Parameters.AddWithValue("@MonthStart", MonthStart);
        //        selectCommmand.Parameters.AddWithValue("@MonthEnd", MonthEnd);

        //        SqliteDataReader query = selectCommmand.ExecuteReader();
        //        //var db = new DataContext(connection);
        //        while (query.Read())
        //        {

        //            //how turn data from row to this -- might be my solution? https://learn.microsoft.com/en-us/dotnet/api/system.data.linq.datacontext.executequery?view=netframework-4.8.1&redirectedfrom=MSDN#System_Data_Linq_DataContext_ExecuteQuery__1_System_String_System_Object___


        //            var temp = new CalendarTaskData(
        //                query.GetString(0),
        //                //query.GetString(1) != null ? true : false,
        //                //query.GetString(2) != null ? true : false,
        //                query.GetString(3),
        //                query.GetBoolean(4),
        //                query.GetDateTime(5),
        //                query.GetDateTime(6));
        //            entries.Add(temp);
        //        }
        //    }
        //    #endregion

        //    return entries;
        //}

        //#endregion

        ///**
        // * Generic Method to be used for prototyping purposes and will ideally be replaced when I get the chance/know more about mvvm,
        // * but for right now need to find a way to avoid the calendar controls knowing about the main view model class
        // */
        //public static List<CalendarTaskData> GetTasksFromSandwichMonths(DateTime CurrentDate)
        //{
        //    DateTime StartDate = CurrentDate.AddMonths(-1);
        //    DateTime EndDate = CurrentDate.AddMonths(1);
        //    //because the month doesn't change years
        //    if (StartDate.Month == 11)
        //        StartDate = StartDate.AddYears(-1);
        //    if (EndDate.Month == 0)
        //        EndDate = EndDate.AddYears(1);
            
        //    var entries = new List<CalendarTaskData>();
        //    #region ONLY FOR TESTING PURPOSES- WILL CHANGE IN FINAL VERSION - HOW DO I DO THIS?
        //    using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
        //    {
        //        connection.Open();
        //        var selectCommmand = new SqliteCommand(
        //            "SELECT * FROM CalendarTasks WHERE DateStart<=@EndDate AND DateEnd>=@StartDate", connection);
        //        //Only getting id, task name, task completion, date start, and date end because those are the ones which the calendars NEED to display
        //        //the guid, and task file name aren't necessarily necessary, and so storing them would be a waste- can instead fetch them from the table
        //        //but maybe can store bool as to whether or not those are empty?


        //        selectCommmand.Parameters.AddWithValue("@StartDate", StartDate);
        //        selectCommmand.Parameters.AddWithValue("@EndDate", EndDate);

        //        SqliteDataReader query = selectCommmand.ExecuteReader();
        //        //var db = new DataContext(connection);
        //        while (query.Read())
        //        {

        //            //how turn data from row to this -- might be my solution? https://learn.microsoft.com/en-us/dotnet/api/system.data.linq.datacontext.executequery?view=netframework-4.8.1&redirectedfrom=MSDN#System_Data_Linq_DataContext_ExecuteQuery__1_System_String_System_Object___


        //            var temp = new CalendarTaskData(
        //                query.GetString(0),
        //                //query.GetString(1) != null ? true : false,
        //                //query.GetString(2) != null ? true : false,
        //                query.GetString(3),
        //                query.GetBoolean(4),
        //                query.GetDateTime(5),
        //                query.GetDateTime(6));
        //            entries.Add(temp);
        //        }


        //    }


        //    #endregion

        //    return entries;
        //}
        //#endregion
    }

}
