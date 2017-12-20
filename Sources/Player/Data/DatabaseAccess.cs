using AlphaCodex.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AlphaCodex.Data
{
    public partial class DatabaseAccess
    {
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
        /// Handle the feats data fetched from the database
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
            SQLiteCommand command = new SQLiteCommand("SELECT grp.* FROM player_favorite AS fav LEFT JOIN player_favorite_group AS grp ON fav.id_group = grp.id WHERE id_player = @id GROUP BY grp.id ORDER BY[name]");
            command.Parameters.AddWithValue("@id", player.Id);

            return ExecuteQuery(command, FavoriteGroupsHandler) as List<FavoriteGroup>;
        }

        /// <summary>
        /// Handle the feats data fetched from the database
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

        public bool SaveFavorite(Favorite favorite)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT INTO player_favorite (id_player, type, id_favorite, id_group) VALUES(@player, @type, @id, @group)");
            command.Parameters.AddWithValue("@player", 1);
            command.Parameters.AddWithValue("@type", favorite.Type);
            command.Parameters.AddWithValue("@id", favorite.Id);
            command.Parameters.AddWithValue("@group", favorite.Group);
            if (ExecuteNonQuery(command) == 0) return false;
            return true;
        }
    }
}