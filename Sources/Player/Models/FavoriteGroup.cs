using System.Collections.Generic;

namespace AlphaCodex
{
    public class FavoriteGroup
    {
        #region Properties
        public string Name { get; set; }
        public List<Favorite> Favorites { get; set; }
        #endregion Properties
        #region constructor
        public FavoriteGroup(string group)
        {
            Name = group;
            Favorites = new List<Favorite>();
        }

        public FavoriteGroup(Favorite favorite)
        {
            Name = favorite.Group;
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
        #endregion Methods
    }
}