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
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'player' WHERE id LIKE 1");

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
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM player_favorite AS fav " +
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
    }
}