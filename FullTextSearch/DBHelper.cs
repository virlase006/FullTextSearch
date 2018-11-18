using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullTextSearch
{
    public class DBHelper
    {
        public bool IsTableExist(SQLiteConnection connection, string tablename)
        {

            bool response = false;
            var commandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tablename}';";
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = commandText;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                response = tablename == reader[0].ToString();
            }
            return response;



        }
        public bool CreateTable(string commandText, string tablename, SQLiteConnection connection)
        {
            try
            {
                if (IsTableExist(connection, tablename))
                {
                    Console.WriteLine("Table already exist");
                    return true;
                }
                else
                {


                    return ExecuteCommand(commandText, connection);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cant create table : " + ex.Message);
                return false;
            }



        }

        public bool ExecuteCommand(string commandText, SQLiteConnection connection)
        {
            try
            {

                SQLiteCommand command = new SQLiteCommand(commandText, connection);
                command.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Cant Execute : " + ex.Message);
                return false;
            }

        }
        public List<KeyValuePair<string, object>> Search(string text, SQLiteConnection connection)
        {
            string cmod = $"SELECT * FROM sports_news_index WHERE news MATCH '{text}' OR heading MATCH '{text}' UNION SELECT * FROM weather_news_index WHERE news MATCH '{text}' OR heading MATCH '{text}'; ";
            List<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();
            SQLiteCommand command = new SQLiteCommand(cmod, connection);
            var reader = command.ExecuteReader();
            int i = 0;
            while (reader.Read())
            { 
     
                result.Add(new KeyValuePair<string, object>($"SearchResult: {i++}", $"{reader[1].ToString()}----{reader[0].ToString()}"));
            }
            return result;

        }

    }



}
