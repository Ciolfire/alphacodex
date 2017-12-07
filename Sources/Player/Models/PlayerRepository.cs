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
        private List<FavoriteGroup> _favoriteGroups = new List<FavoriteGroup>();

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
            InitializeFavoriteGroups(da.GetFavorites(_player));
        }

        #region Methods
        /// <summary>
        /// Sort the favorites by group
        /// </summary>
        /// <param name="favorites"></param>
        private void InitializeFavoriteGroups(List<Favorite> favorites)
        {
            if (favorites == null) return;
            var start = DateTime.Now;
            FavoriteGroup group;
            foreach (Favorite favorite in favorites)
            {
                group = _favoriteGroups.SingleOrDefault(item => item.Name == favorite.Group);
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
                            favorite.Name = main.Feats.GetFeats().Single(feat => feat.Id == favorite.Id).Name;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        #endregion
    }
}
