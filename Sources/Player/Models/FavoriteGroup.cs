using AlphaCodex.Data;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AlphaCodex
{
    public class FavoriteGroup
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Favorite> Favorites { get; set; }
        #endregion Properties
        #region constructor
        public FavoriteGroup(string group)
        {
            Name = group;
            Favorites = new List<Favorite>();
        }

        public FavoriteGroup(SQLiteDataReader reader)
        {
            Id = (int)(long)reader["id"];
            Name = reader["name"] as string;
            Favorites = new List<Favorite>();
        }

        public FavoriteGroup(Favorite favorite)
        {
            Id = favorite.Group;
            Name = "Group id: " + Id.ToString();
            Favorites = new List<Favorite>
            {
                favorite
            };
        }
        #endregion constructor
        #region Methods
        public void Add(Favorite favorite)
        {
            if (!Favorites.Exists(item => item == favorite))
            {
                Favorites.Add(favorite);
            }
        }
        public bool Save()
        {
            DatabaseAccess da = new DatabaseAccess();
            int id = da.NewFavoriteGroup(Name);
            if (id == -1) return false;
            Id = id;
            return true;
        }

        internal void Delete()
        {
            DatabaseAccess da = new DatabaseAccess();

            da.DeleteFavoriteGroup(this);
        }
        #endregion Methods
    }
}