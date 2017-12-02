using AlphaCodex.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaCodex.ViewModels
{
    class PlayerPanelViewModel
    {
        #region Fields
        private PlayerRepository _playerRepository;
        public PlayerViewModel _player;
        private ObservableCollection<FavoriteGroupViewModel> _favoriteGroups;
        #endregion
        #region Properties
        public PlayerViewModel Player => _player;
        public ObservableCollection<FavoriteGroupViewModel> FavoriteGroups
        {
            get
            {
                if (_favoriteGroups == null)
                {
                    _favoriteGroups = new ObservableCollection<FavoriteGroupViewModel>();
                }
                return _favoriteGroups;
            }
        }
        #endregion
        #region Constructor
        public PlayerPanelViewModel(PlayerRepository repository)
        {
            _playerRepository = repository;
            _player = new PlayerViewModel(repository.GetPlayer());
            foreach (FavoriteGroup group in repository.GetFavoriteGroups())
            {
                FavoriteGroupViewModel newItem = new FavoriteGroupViewModel(group);
                FavoriteGroups.Add(newItem);
            }
        }
        #endregion
        #region Methods
        #endregion
    }
}
