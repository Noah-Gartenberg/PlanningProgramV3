using Microsoft.Data.Sqlite;
using Microsoft.Windows;
using PlanningProgramV3.Models;
using System;
using System.Collections.Generic;
//using System.Data.Linq;
using System.IO;
using System.Windows;
using Windows.Management.Core;
using Windows.Storage;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace PlanningProgramV3.ViewModels.Calendar
{
    //https://learn.microsoft.com/en-us/windows/apps/develop/data-access/sqlite-data-access
    public static class DataAccess
    {

        public static void InitializeDatabase()
        {

            #region For testing purposes only - delete in final -- how to make work in final only????


            #endregion
            using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
            {
                connection.Open();

                //so calendar calendarTasks need a few columns: date start, date end, task name, completion, task id, filename????

                var command = connection.CreateCommand();
                //thinking here: maybe I want a table for tasks that have date durations, and then a table for the actual start and end dates themselves - that way I'm not saving unnecessary data
                command.CommandText =
                    @"
                        CREATE TABLE IF NOT EXISTS CalendarTasks(
                            tableGUID VARCHAR(36) PRIMARY KEY,
                            taskGUID VARCHAR(36),
                            TaskFileName TEXT,
                            TaskName TEXT NOT NULL,
                            TaskCompletion BOOLEAN NOT NULL,
                            DateStart DATE NOT NULL,
                            DateEnd DATE NOT NULL
                    );
                    INSERT OR REPLACE INTO CalendarTasks VALUES ('TABLEGUID1','GUID1', 'TaskFileName', 'TaskName1', 1, '2025-08-26 00:00:00.000','2025-08-27 00:00:00.000'),
                                                                ('TABLEGUID2','GUID2', 'TaskFileName', 'TaskName2', 0, '2025-08-28 00:00:00.000','2025-08-31 00:00:00.000'),
                                                                ('TABLEGUID3','GUID3', 'TaskFileName', 'TaskName3', 0, '2025-08-27 00:00:00.000', '2025-09-10 00:00:00.000');";
                //FURTHER TEST STUFF
                //('GUID2', 'TaskFileName', 'TaskName2', 0, '2025-08-28 00:00:00.000','2025-08-31 00:00:00.000'),
                //('GUID3', 'TaskFileName', 'TaskName3', 0, '2025-08-27 00:00:00.000', '2025-09-01 00:00:00.000')
                System.Console.WriteLine(connection.ConnectionString);
                command.ExecuteNonQuery();
            }
        }

        public static void AddTask(ref DateDurationModelData CalendarTaskData)
        {
            
        }

        public static void AddTask(ref string guid, ref string taskFileName, ref string TaskName, bool taskCompletion, ref DateTime dateStart, ref DateTime dateEnd)
        {

        }

        #region Methods for getting the Data
        //TABLE FOR TELLING IF A TASK IS WITHIN A PERIOD OF TIME;
        //ASSUME THAT THE TASK'S END DATE IS MORE THAN OR EQUAL TO THE TASK START DATE
        //BOTH START AND END DATES ARE INCLUSIVE
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
         * |     Task End Date   MUST NOT be before Time Period Start                                                         |
         * |==================================================================================================================|
         *      
         */
        public static List<CalendarTaskData> GetTasksFromDate(DateTime Date)
        {
            var entries = new List<CalendarTaskData>();
            #region ONLY FOR TESTING PURPOSES- WILL CHANGE IN FINAL VERSION - HOW DO I DO THIS?
            using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
            {
                connection.Open();
                var selectCommmand = new SqliteCommand(
                    "SELECT * FROM CalendarTasks WHERE DateStart<=@Date AND DateEnd>=@Date;", connection);
                //Only getting id, task name, task completion, date start, and date end because those are the ones which the calendars NEED to display
                    //the guid, and task file name aren't necessarily necessary, and so storing them would be a waste- can instead fetch them from the table
                    //but maybe can store bool as to whether or not those are empty?
                selectCommmand.Parameters.AddWithValue("@Date", Date);

                SqliteDataReader query = selectCommmand.ExecuteReader();
                //var db = new DataContext(connection);
                while (query.Read())
                {

                    //how turn data from row to this -- might be my solution? https://learn.microsoft.com/en-us/dotnet/api/system.data.linq.datacontext.executequery?view=netframework-4.8.1&redirectedfrom=MSDN#System_Data_Linq_DataContext_ExecuteQuery__1_System_String_System_Object___
                    

                    var temp = new CalendarTaskData(
                        query.GetString(0),
                        query.GetString(1) != null ? true : false, 
                        query.GetString(2) != null ? true : false, 
                        query.GetString(3), 
                        query.GetBoolean(4), 
                        query.GetDateTime(5), 
                        query.GetDateTime(5));
                    entries.Add(temp);
                }
            }
            #endregion

            return entries;
        }

        public static List<CalendarTaskData> GetTasksFromWeek(DateTime WeekStart, DateTime WeekEnd)
        {
            var entries = new List<CalendarTaskData>();
            #region ONLY FOR TESTING PURPOSES- WILL CHANGE IN FINAL VERSION - HOW DO I DO THIS?
            using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
            {
                connection.Open();
                var selectCommmand = new SqliteCommand(
                    "SELECT * FROM CalendarTasks WHERE DateStart<=@WeekEnd AND DateEnd>=@WeekStart;", connection);
                //Only getting id, task name, task completion, date start, and date end because those are the ones which the calendars NEED to display
                //the guid, and task file name aren't necessarily necessary, and so storing them would be a waste- can instead fetch them from the table
                //but maybe can store bool as to whether or not those are empty?
                selectCommmand.Parameters.AddWithValue("@WeekStart", WeekStart);
                selectCommmand.Parameters.AddWithValue("@WeekEnd", WeekEnd);

                SqliteDataReader query = selectCommmand.ExecuteReader();
                //var db = new DataContext(connection);
                while (query.Read())
                {

                    //how turn data from row to this -- might be my solution? https://learn.microsoft.com/en-us/dotnet/api/system.data.linq.datacontext.executequery?view=netframework-4.8.1&redirectedfrom=MSDN#System_Data_Linq_DataContext_ExecuteQuery__1_System_String_System_Object___

                    
                    var temp = new CalendarTaskData(
                        query.GetString(0),
                        query.GetString(1) != null ? true : false,
                        query.GetString(2) != null ? true : false,
                        query.GetString(3),
                        query.GetBoolean(4),
                        query.GetDateTime(5),
                        query.GetDateTime(6));
                    entries.Add(temp);
                }
            }
            #endregion

            return entries;
        }

        //LITERALLY FOR THE LOVE OF G-D, DO NOT USE THIS OUTSIDE OF TESTING PURPOSES!!!
        public static List<CalendarTaskData> GetAllTasks()
        {
            var entries = new List<CalendarTaskData>();
            #region ONLY FOR TESTING PURPOSES- WILL CHANGE IN FINAL VERSION - HOW DO I DO THIS?
            using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
            {
                connection.Open();
                var selectCommmand = new SqliteCommand(
                    "SELECT * FROM CalendarTasks WHERE 1;", connection);
                //Only getting id, task name, task completion, date start, and date end because those are the ones which the calendars NEED to display
                //the guid, and task file name aren't necessarily necessary, and so storing them would be a waste- can instead fetch them from the table when requested
                //but maybe can store bool as to whether or not those are empty?

                SqliteDataReader query = selectCommmand.ExecuteReader();
                //var db = new DataContext(connection);
                while (query.Read())
                {

                    //how turn data from row to this -- might be my solution? https://learn.microsoft.com/en-us/dotnet/api/system.data.linq.datacontext.executequery?view=netframework-4.8.1&redirectedfrom=MSDN#System_Data_Linq_DataContext_ExecuteQuery__1_System_String_System_Object___


                    var temp = new CalendarTaskData(
                        query.GetString(0),
                        query.GetString(1) != null ? true : false,
                        query.GetString(2) != null ? true : false,
                        query.GetString(3),
                        query.GetBoolean(4),
                        query.GetDateTime(5),
                        query.GetDateTime(6));
                    entries.Add(temp);
                }
            }
            #endregion

            return entries;
        }

        public static List<CalendarTaskData> GetTasksFromMonth(DateTime MonthStart, DateTime MonthEnd)
        {
            var entries = new List<CalendarTaskData>();
            #region ONLY FOR TESTING PURPOSES- WILL CHANGE IN FINAL VERSION - HOW DO I DO THIS?
            using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
            {
                connection.Open();
                var selectCommmand = new SqliteCommand(
                    "SELECT * FROM CalendarTasks WHERE DateStart<=@MonthEnd AND DateEnd>=@MonthStart", connection);
                //Only getting id, task name, task completion, date start, and date end because those are the ones which the calendars NEED to display
                //the guid, and task file name aren't necessarily necessary, and so storing them would be a waste- can instead fetch them from the table
                //but maybe can store bool as to whether or not those are empty?
                    

                selectCommmand.Parameters.AddWithValue("@MonthStart", MonthStart);
                selectCommmand.Parameters.AddWithValue("@MonthEnd", MonthEnd);

                SqliteDataReader query = selectCommmand.ExecuteReader();
                //var db = new DataContext(connection);
                while (query.Read())
                {

                    //how turn data from row to this -- might be my solution? https://learn.microsoft.com/en-us/dotnet/api/system.data.linq.datacontext.executequery?view=netframework-4.8.1&redirectedfrom=MSDN#System_Data_Linq_DataContext_ExecuteQuery__1_System_String_System_Object___


                    var temp = new CalendarTaskData(
                        query.GetString(0),
                        query.GetString(1) != null ? true : false,
                        query.GetString(2) != null ? true : false,
                        query.GetString(3),
                        query.GetBoolean(4),
                        query.GetDateTime(5),
                        query.GetDateTime(6));
                    entries.Add(temp);
                }
            }
            #endregion

            return entries;
        }

        #endregion

        /**
         * Generic Method to be used for prototyping purposes and will ideally be replaced when I get the chance/know more about mvvm,
         * but for right now need to find a way to avoid the calendar controls knowing about the main view model class
         */
        public static List<CalendarTaskData> GetTasksFromSandwichMonths(DateTime CurrentDate)
        {
            DateTime StartDate = CurrentDate.AddMonths(-1);
            DateTime EndDate = CurrentDate.AddMonths(1);
            //because the month doesn't change years
            if (StartDate.Month == 11)
                StartDate = StartDate.AddYears(-1);
            if (EndDate.Month == 0)
                EndDate = EndDate.AddYears(1);
            
            var entries = new List<CalendarTaskData>();
            #region ONLY FOR TESTING PURPOSES- WILL CHANGE IN FINAL VERSION - HOW DO I DO THIS?
            using (var connection = new SqliteConnection("Data Source=CalendarTask_Database.db"))
            {
                connection.Open();
                var selectCommmand = new SqliteCommand(
                    "SELECT * FROM CalendarTasks WHERE DateStart<=@EndDate AND DateEnd>=@StartDate", connection);
                //Only getting id, task name, task completion, date start, and date end because those are the ones which the calendars NEED to display
                //the guid, and task file name aren't necessarily necessary, and so storing them would be a waste- can instead fetch them from the table
                //but maybe can store bool as to whether or not those are empty?


                selectCommmand.Parameters.AddWithValue("@StartDate", StartDate);
                selectCommmand.Parameters.AddWithValue("@EndDate", EndDate);

                SqliteDataReader query = selectCommmand.ExecuteReader();
                //var db = new DataContext(connection);
                while (query.Read())
                {

                    //how turn data from row to this -- might be my solution? https://learn.microsoft.com/en-us/dotnet/api/system.data.linq.datacontext.executequery?view=netframework-4.8.1&redirectedfrom=MSDN#System_Data_Linq_DataContext_ExecuteQuery__1_System_String_System_Object___


                    var temp = new CalendarTaskData(
                        query.GetString(0),
                        query.GetString(1) != null ? true : false,
                        query.GetString(2) != null ? true : false,
                        query.GetString(3),
                        query.GetBoolean(4),
                        query.GetDateTime(5),
                        query.GetDateTime(6));
                    entries.Add(temp);
                }


            }


            #endregion

            return entries;
        }
    }
}
