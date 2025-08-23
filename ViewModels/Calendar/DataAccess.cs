using Microsoft.Data.Sqlite;
using Microsoft.Windows;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Windows.Storage;


namespace PlanningProgramV3.ViewModels.Calendar
{
    //https://learn.microsoft.com/en-us/windows/apps/develop/data-access/sqlite-data-access
    public static class DataAccess
    {

        public async static void InitializeDatabase()
        {
            /**
             * For finished version, use ApplicationData
             */
            await ApplicationData.Current.LocalFolder.CreateFileAsync("CalendarTasks.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "CalendarTasks.db");

            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                string tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, " +
                    "Text_Entry NVARCHAR(2048) NULL)";

                var createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddData(string inputText)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path,
                                 "CalendarTasks.db");
            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry);";
                insertCommand.Parameters.AddWithValue("@Entry", inputText);

                insertCommand.ExecuteReader();
            }
        }

        public static List<string> GetData()
        {
            var entries = new List<string>();
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path,
                                         "CalendarTasks.db");
            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                var selectCommand = new SqliteCommand
                    ("SELECT Text_Entry from MyTable", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                }
            }

            return entries;
        }
    }
}
