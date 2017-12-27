using AlphaCodex.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AlphaCodex.Data
{
    public partial class DatabaseAccess
    {
        /// <summary>
        /// Save a new player
        /// </summary>
        public bool SavePlayer(string name)
        {
            SQLiteCommand command = new SQLiteCommand("UPDATE player SET name = @name WHERE id = 1");
            command.Parameters.AddWithValue("@name", name);
            if (ExecuteNonQuery(command) == 0) return false;
            return true;
        }

        /// <summary>
        /// Retrieve the player from the database
        /// </summary>
        public Player GetPlayer()
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'player' WHERE id LIKE @id");
            command.Parameters.AddWithValue("@id", 1);

            return ExecuteQuery(command, PlayerHandler) as Player;
        }

        /// <summary>
        /// Handle the player data fetched from the database
        /// </summary>
        /// <returns></returns>
        private Object PlayerHandler()
        {
            reader.Read();
            Player player = new Player(reader);
            System.Diagnostics.Debug.WriteLine("[Player] Fetched.");
            return player;
        }

        /// <summary>
        /// Retrieve the Favorites from the database
        /// </summary>
        public List<Favorite> GetFavorites(Player player)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT fav.* FROM player_favorite AS fav " +
                "LEFT JOIN player_favorite_group AS grp ON fav.id_group = grp.id " +
                "WHERE id_player = 1 ORDER BY[name]");
            command.Parameters.AddWithValue("@id", player.Id);

            return ExecuteQuery(command, FavoritesHandler) as List<Favorite>;
        }

        /// <summary>
        /// Handle the favorites data fetched from the database
        /// </summary>
        /// <returns></returns>
        private Object FavoritesHandler()
        {
            List<Favorite> favorites = new List<Favorite>();

            while (reader.Read())
            {
                favorites.Add(new Favorite(reader));
            }
            System.Diagnostics.Debug.WriteLine("[Player] Favorites Fetched.");
            return favorites;
        }

        /// <summary>
        /// Retrieve the Favorites from the database
        /// </summary>
        public List<FavoriteGroup> GetFavoriteGroups(Player player)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT id,name FROM player_favorite_group WHERE player_id = @id OR player_id = 0 ORDER BY[name]");
            command.Parameters.AddWithValue("@id", player.Id);

            return ExecuteQuery(command, FavoriteGroupsHandler) as List<FavoriteGroup>;
        }

        /// <summary>
        /// Handle the favorite groups data fetched from the database
        /// </summary>
        /// <returns></returns>
        private Object FavoriteGroupsHandler()
        {
            List<FavoriteGroup> groups = new List<FavoriteGroup>();

            while (reader.Read())
            {
                groups.Add(new FavoriteGroup(reader));
            }
            System.Diagnostics.Debug.WriteLine("[Player] Favorite Groups Fetched.");
            return groups;
        }

        /// <summary>
        /// Create a new favorite group
        /// </summary>
        internal int NewFavoriteGroup(string name)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT INTO player_favorite_group (name, player_id) VALUES (@name, @id)");
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@id", 1);
            if (ExecuteNonQuery(command) == 0) return -1;
            command = new SQLiteCommand("SELECT id FROM player_favorite_group WHERE name = @name");
            command.Parameters.AddWithValue("@name", name);
            return (int)ExecuteQuery(command, NewFavoriteGroupHandler);
        }

        private Object NewFavoriteGroupHandler()
        {
            reader.Read();
            return (int)(long)reader["id"];
        }

        /// <summary>
        /// Update a favorite group name
        /// </summary>
        public bool UpdateFavoriteGroup(FavoriteGroup group, string newName)
        {
            SQLiteCommand command = new SQLiteCommand("UPDATE player_favorite_group SET name = @name WHERE id = @id AND name != @name");
            command.Parameters.AddWithValue("@id", group.Id);
            command.Parameters.AddWithValue("@name", newName);
            if (ExecuteNonQuery(command) == 0) return false;
            return true;
        }

        /// <summary>
        /// Remove a favorite group
        /// </summary>
        public bool DeleteFavoriteGroup(FavoriteGroup group)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM player_favorite_group WHERE id = @id");
            command.Parameters.AddWithValue("@id", group.Id);
            if (ExecuteNonQuery(command) == 0) return false;
            command.CommandText = "DELETE FROM player_favorite WHERE id_group = @id";
            ExecuteNonQuery(command);
            return true;
        }

        /// <summary>
        /// Save a new favorite
        /// </summary>
        public int SaveFavorite(Favorite favorite)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT INTO player_favorite (id_player, type, id_favorite, id_group) VALUES(@player, @type, @favorite, @group)");
            command.Parameters.AddWithValue("@player", 1);
            command.Parameters.AddWithValue("@type", favorite.Type);
            command.Parameters.AddWithValue("@favorite", favorite.ParentId);
            command.Parameters.AddWithValue("@group", favorite.Group);
            if (ExecuteNonQuery(command) == 0) return -1;
            command = new SQLiteCommand("select id FROM player_favorite ORDER BY id DESC LIMIT 1");
            return (int)ExecuteQuery(command, NewFavoriteHandler);
        }

        private Object NewFavoriteHandler()
        {
            reader.Read();
            return (int)(long)reader["id"];
        }

        /// <summary>
        /// Remove a favorite
        /// </summary>
        public bool DeleteFavorite(int id)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM player_favorite WHERE id = @id");
            command.Parameters.AddWithValue("@id", id);
            if (ExecuteNonQuery(command) == 0) return false;
            return true;
        }
    }
}