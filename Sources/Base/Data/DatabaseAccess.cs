using AlphaCodex.Models;
using AlphaCodex.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace AlphaCodex.Data
{
    /// <summary>   
    /// Database access class.  
    /// </summary>   
    public partial class DatabaseAccess
    {
        #region Fields
        private string ConnectionString;
        private SQLiteDataReader reader;
        #endregion
        #region Constructors
        public DatabaseAccess()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            ConnectionString = "Data Source=" + path + @"\Data\LyndisLibrary.sqlite;Version=3;";
        }
        #endregion
        #region Methods
        /// <summary>
        /// Execute a query string and handle the result with a function
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="dataHandler"></param>
        /// <returns></returns>
        private Object ExecuteQuery(SQLiteCommand command, Func<Object> dataHandler)
        {
            Object result;
            using (SQLiteConnection connDB = new SQLiteConnection(ConnectionString))
            {
                connDB.Open();
                command.Connection = connDB;
                reader = command.ExecuteReader();
                result = dataHandler();
                reader.Close();
                connDB.Close();
            }
            return result;
        }

        private int ExecuteNonQuery(SQLiteCommand command)
        {
            int result;
            using (SQLiteConnection connDB = new SQLiteConnection(ConnectionString))
            {
                connDB.Open();
                command.Connection = connDB;
                result = command.ExecuteNonQuery();
                connDB.Close();
            }
            return result;
        }

        /// <summary>
        /// Retrieve a table from the DB
        /// </summary>
        public DataTable GetTable(string type, int id)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM " + type + "_table WHERE table_id = @id");
            command.Parameters.AddWithValue("@id", id);
            return JadeTools.CreateTable(ExecuteQuery(command, TableHandler) as List<Column>);
        }

        /// <summary>
        /// Handle the table data fetched from the database
        /// </summary>
        /// <returns></returns>
        private Object TableHandler()
        {
            List<Column> columns = new List<Column>();

            while (reader.Read())
            {
                columns.Add(new Column(reader));
            }
            return columns;
        }
        #endregion
    }
}
