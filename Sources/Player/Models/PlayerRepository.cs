using AlphaCodex.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaCodex.Models
{
    public class PlayerRepository
    {
        #region Fields
        private Player _player;
        private List<FavoriteGroup> _favoriteGroups;

        #endregion
        #region Properties
        public Player GetPlayer()
        {
            return _player;
        }
        public List<FavoriteGroup> GetFavoriteGroups()
        {
            return _favoriteGroups;
        }

        #endregion

        public PlayerRepository()
        {
            DatabaseAccess da = new DatabaseAccess();

            _player = da.GetPlayer();
            _favoriteGroups = da.GetFavoriteGroups(_player);
            AssignFavoritesToGroups(da.GetFavorites(_player));
        }

        #region Methods
        /// <summary>
        /// Sort the favorites by group
        /// </summary>
        /// <param name="favorites"></param>
        private void AssignFavoritesToGroups(List<Favorite> favorites)
        {
            if (favorites == null) return;
            var start = DateTime.Now;
            FavoriteGroup group;
            foreach (Favorite favorite in favorites)
            {
                group = _favoriteGroups.SingleOrDefault(item => item.Id == favorite.Group);
                if (group == null)
                {
                    _favoriteGroups.Add(new FavoriteGroup(favorite));
                } else
                {
                    group.Add(favorite);
                }
            }
            System.Diagnostics.Debug.WriteLine("[Player] Favorites groups applied in {0} ms.", (DateTime.Now - start).TotalMilliseconds);
        }

        public void SetFavoritesNames(MainRepository main)
        {
            foreach (FavoriteGroup group in _favoriteGroups)
            {
                foreach (Favorite favorite in group.Favorites)
                {
                    switch (favorite.Type)
                    {
                        case "feat":
                            favorite.Name = main.Feats.GetFeats().Single(feat => feat.Id == favorite.ParentId).Name;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        internal void SaveFavorite(Favorite favorite)
        {
            _favoriteGroups.Single(group => group.Id == favorite.Group).Add(favorite);
        }
        #endregion
    }
}
