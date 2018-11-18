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
            Console.Write(Resource1.EnterSearchText);
            var text = Console.ReadLine();
            var result = DbHelper.Search(text, con);
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }
            Console.WriteLine(Resource1.ChooseAction);
            Console.WriteLine(Resource1.SearchMore);
            Console.WriteLine(Resource1.GoBack);
            Console.WriteLine(Resource1.AnyOtherKey);
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
            Console.WriteLine(Resource1.ChooseTable);
            Console.WriteLine(Resource1.SportsTable);
            Console.WriteLine(Resource1.WeatherTable);
            var table = Console.ReadLine();
            if (table == "1" || table == "2")
            {
                var tableName = table == "1" ? sportsNewTable : weatherNewsTable;
                Console.WriteLine(Resource1.ChooseCRUD);
                Console.WriteLine("1-Create");
                Console.WriteLine("2-Update");
                Console.WriteLine("3-Delete");
                var resp = Console.ReadLine();
                PerfromCRUD(tableName, resp);
            }
            else Console.WriteLine(Resource1.InvalidSelection);


        }

        private static void PerfromCRUD(string tableName, string resp)
        {
            switch (resp)
            {
                case "1":
                    CreateNewRecord(tableName);
                    Console.WriteLine(Resource1.CreateRecord);
                    break;
                case "2":
                    UpdateRecord(tableName);
                    Console.WriteLine(Resource1.UpdatedRecorded);
                    break;
                case "3":
                    DeleteRecord(tableName);
                    Console.WriteLine(Resource1.DeletedRecord);
                    break;
                default:
                    Console.WriteLine(Resource1.InvalidSelection);
                    break;
            }
        }

        private static void DeleteRecord(string tableName)
        {
            Console.WriteLine(Resource1.EnterRecIdForDelete);
            var recordId = Console.ReadLine();
            DbHelper.ExecuteCommand($"DELETE from  {tableName} WHERE id = {recordId};", con);

        }

        private static void CreateNewRecord(string tableName)
        {
            Console.WriteLine(Resource1.EnterValueForCOlumn.Replace("_columnName_","heaeding"));
            var heading = Console.ReadLine();
            Console.WriteLine(Resource1.EnterValueForCOlumn.Replace("_columnName_", "news"));

            var news = Console.ReadLine();
            DbHelper.ExecuteCommand($"INSERT INTO {tableName} (news, heading) VALUES ('{news}','{heading}');", con);
        }

        private static void UpdateRecord(string tableName)
        {
            Console.WriteLine(Resource1.EnterRecIdForUpdate);
            var recordId = Console.ReadLine();
            Console.WriteLine(Resource1.ChooseColumn);
            var column = Console.ReadLine();

            if (column == "1" || column == "2")
            {
                var columnName = column == "1" ? "news" : "heading";
                Console.WriteLine(Resource1.EnterValueForCOlumn.Replace("_columnName_",columnName));
                var newValue = Console.ReadLine();
                Console.WriteLine($"UPDATE  {tableName} SET {columnName}='{newValue}' WHERE id={recordId};");
                DbHelper.ExecuteCommand($"UPDATE  {tableName} SET {columnName}='{newValue}' WHERE id={recordId};", con);
            }
            else
            {
                Console.WriteLine(Resource1.InvalidSelection);
            }

        }

        private static void StartingScreen()
        {
            Console.WriteLine(Resource1.ChooseAction);
            Console.WriteLine(Resource1.InitializeDb);
            Console.WriteLine(Resource1.PerformCRUD);
            Console.WriteLine(Resource1.SearchText);
            var response = Console.ReadLine();
            PerformAction(DbHelper, con, response);
        }

        private static void InitializeDatabaseForSearching(DBHelper dbHelper, SQLiteConnection con)
        {
            Console.WriteLine(Resource1.ChooseDBActions);
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
                Console.WriteLine(Resource1.InvalidSelection);
            }
            
        }


    }

}
