using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AlphaCodex
{
    public class Player
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, Favorite> Favorites { get; set; }
        #endregion

        #region constructor
        public Player(SQLiteDataReader reader)
        {
            Id = (int)(long)reader["id"];
            Name = reader["name"] as string;
            SetFavorites();
        }

        internal void SetFavorites()
        {
            string queryUpdate = "SELECT * FROM player_favorite WHERE id_player = @id";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                Favorites = new Dictionary<string, Favorite>();

                connDB.Close();
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryUpdate, connDB);
                command.Parameters.Add(new SQLiteParameter("@id", Id));
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Need to find a solution that include both observation and good indexing, maybe create ObservableDictionary ?
                    // Otherwise the players favorites are not updated in real time. Priority.
                    AddFavoriteFromDB(reader);
                }
                reader.Close();
                connDB.Close();
            }
        }

        private void AddFavoriteFromDB(SQLiteDataReader reader)
        {
            string group = reader["group"] as string;
            string type = reader["type"] as string;
            int id = (int)(long)reader["id"];
            if (group == null || group == string.Empty)
            {
                // not elegant, to be improved
                group = "DEFAULT_GROUP";
            }
            Favorites.Add(group, new Favorite(id, type));
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

        public class Favorite
        {
            string Type { get; set; }
            //string Group { get; set; } <- should be set directly in the dictionary, scrapped from this.
            int Id { get; set; }

            public Favorite(int id, string type)
            {
                Type = type;
                Id = id;
            }
        }
        #endregion
    }
}