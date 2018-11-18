using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
namespace FullTextSearch
{
    class Program
    {
        static string datasource = "Data Source=TestNewsDB;Version=3;";
        static SQLiteConnection con = new SQLiteConnection(datasource);
        static DBHelper DbHelper = new DBHelper();
        static string sportsNewTable = "sports_news";
        static string weatherNewsTable = "weather_news";

        static void Main(string[] args)
        {

            try
            {
                con.Open();
                con.EnableExtensions(true);
                StartingScreen();
                con.Close();
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                Console.Read();
            }
        }

        private static void PerformAction(DBHelper DbHelper, SQLiteConnection con, string response)
        {
            switch (response)
            {
                case "1":
                    InitializeDatabaseForSearching(DbHelper, con);
                    StartingScreen();
                    break;
                case "2":
                    PerformCrudOpertaion();
                    StartingScreen();
                    break;
                case "3":
                    PerformSearch();
                    break;

            }
        }

        private static void PerformSearch()
        {
            Console.Write("Enter text you would like to search: ");
            var text = Console.ReadLine();
            var result = DbHelper.Search(text, con);
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("1- Search another text");
            Console.WriteLine("2- Go back to start screen");
            Console.WriteLine("Any key to exit");
            var resp = Console.ReadLine();
            if (resp == "1")
            {
                PerformSearch();
            }
            if (resp == "2")
            {
                StartingScreen();
            }
            else
            {
                Console.ReadLine();
            }
        }

        private static void PerformCrudOpertaion()
        {
            Console.WriteLine("Choose the table you would like to perform CRUD Operations");
            Console.WriteLine("1- Sports News");
            Console.WriteLine("2- Weather News");
            var table = Console.ReadLine();
            if (table == "1" || table == "2")
            {
                var tableName = table == "1" ? sportsNewTable : weatherNewsTable;
                Console.WriteLine("Chose CRUD");
                Console.WriteLine("1-Create");
                Console.WriteLine("2-Update");
                Console.WriteLine("3-Delete");
                var resp = Console.ReadLine();
                PerfromCRUD(tableName, resp);
            }
            else Console.WriteLine("Invalid Selection");


        }

        private static void PerfromCRUD(string tableName, string resp)
        {
            switch (resp)
            {
                case "1":
                    CreateNewRecord(tableName);
                    Console.WriteLine("Created record");
                    break;
                case "2":
                    UpdateRecord(tableName);
                    Console.WriteLine("Updated record");
                    break;
                case "3":
                    DeleteRecord(tableName);
                    Console.WriteLine("Deleted record");
                    break;
                default:
                    Console.WriteLine("Invalid options");
                    break;
            }
        }

        private static void DeleteRecord(string tableName)
        {
            Console.WriteLine("Enter record ID you would like to delete");
            var recordId = Console.ReadLine();
            DbHelper.ExecuteCommand($"DELETE from  {tableName} WHERE id = {recordId};", con);

        }

        private static void CreateNewRecord(string tableName)
        {
            Console.WriteLine("Enter heading:");
            var heading = Console.ReadLine();
            Console.WriteLine("Enter news: ");
            var news = Console.ReadLine();
            DbHelper.ExecuteCommand($"INSERT INTO {tableName} (news, heading) VALUES ('{news}','{heading}');", con);
        }

        private static void UpdateRecord(string tableName)
        {
            Console.WriteLine("Enter record Id you would like  to edit");
            var recordId = Console.ReadLine();
            Console.WriteLine("Choose column you would like to update \n 1- news \n 2- heading");
            var column = Console.ReadLine();

            if (column == "1" || column == "2")
            {
                var columnName = column == "1" ? "news" : "heading";
                Console.WriteLine($"Enter new value for the {columnName}");
                var newValue = Console.ReadLine();
                Console.WriteLine($"UPDATE  {tableName} SET {columnName}='{newValue}' WHERE id={recordId};");
                DbHelper.ExecuteCommand($"UPDATE  {tableName} SET {columnName}='{newValue}' WHERE id={recordId};", con);
            }

        }

        private static void StartingScreen()
        {
            Console.WriteLine("Choose from given options");
            Console.WriteLine("1- Initialize database");
            Console.WriteLine("2- Perform CRUD operations");
            Console.WriteLine("3- Full text search");
            var response = Console.ReadLine();
            PerformAction(DbHelper, con, response);
        }

        private static void InitializeDatabaseForSearching(DBHelper dbHelper, SQLiteConnection con)
        {
            Console.WriteLine("Choose what you want to do:\n 1- Create tables and indecis \n 2- Batch insert");
            var resp = Console.ReadLine();
            if (resp == "2" || resp == "1")
            {
                string filename = resp=="1"? "../../../dbInitialization.txt":"../../../batchInsert.txt";

                var commands = System.IO.File.ReadAllLines(filename,Encoding.UTF8);
                foreach (var line in commands)
                {
                    DbHelper.ExecuteCommand(line, con);
                }
            }
            else
            {
                Console.WriteLine("Invalide Operation");
            }
            
        }


    }

}
