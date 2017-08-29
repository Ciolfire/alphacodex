using System;
using System.Data.SQLite;

namespace AlphaCodex
{
    public class Player
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        #endregion

        #region constructor
        public Player(SQLiteDataReader reader)
        {
            Id = (int)(long)reader["id"];
            Name = reader["name"] as string;
        }

        internal void SetName(string text)
        {
            Name = text;

            string queryUpdate = "UPDATE player SET 'name' = @name WHERE player.id = @id";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryUpdate, connDB);
                command.Parameters.Add(new SQLiteParameter("@id", Id));
                command.Parameters.Add(new SQLiteParameter("@name", Name));

                command.ExecuteNonQuery();
                connDB.Close();
            }
        }
        #endregion
    }
}