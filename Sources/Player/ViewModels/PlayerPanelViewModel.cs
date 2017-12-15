using AlphaCodex.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaCodex.Events;
using AlphaCodex.Tools;
using System.Windows.Input;
using static AlphaCodex.ViewModels.MainViewModel;

namespace AlphaCodex.ViewModels
{
    public class PlayerPanelViewModel
    {
        #region Fields
        private MainViewModel _root;
        private PlayerRepository _playerRepository;
        public PlayerViewModel _player;
        private ObservableCollection<FavoriteGroupViewModel> _favoriteGroups;
        #endregion
        #region Properties
        public PlayerViewModel Player => _player;
        public PlayerRepository Repository => _playerRepository;

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
        #region Commands
        RelayCommand _addFavoriteCommand;
        public ICommand AddFavoriteCommand
        {
            get
            {
                if (_addFavoriteCommand == null)
                {
                    _addFavoriteCommand = new RelayCommand(param => AddFavorite(param));
                }
                return _addFavoriteCommand;
            }
        }
        #endregion
        #region Constructor
        public PlayerPanelViewModel(PlayerRepository repository, MainViewModel root)
        {
            _root = root;
            _playerRepository = repository;
            _player = new PlayerViewModel(repository.GetPlayer());
            foreach (FavoriteGroup group in repository.GetFavoriteGroups())
            {
                FavoriteGroupViewModel newItem = new FavoriteGroupViewModel(group);
                foreach (LinkViewModel link in newItem.Favorites)
                {
                    link.LinkClicked += _root.LinkClickedEventHandler;
                }
                newItem.Action += ActionEventHandler;
                FavoriteGroups.Add(newItem);
            }
        }
        #endregion
        #region Methods
        private void ActionEventHandler(object sender, ActionEventArgs args)
        {
            var group = sender as FavoriteGroupViewModel;
            //System.Diagnostics.Debug.WriteLine(args.Action + group.Name);
            switch (args.Action)
            {
                case "delete":
                    _favoriteGroups.Remove(group);
                    break;
                case "duplicate":
                    FavoriteGroupViewModel newItem = new FavoriteGroupViewModel(group);
                    foreach (LinkViewModel link in newItem.Favorites)
                    {
                       // link.LinkClicked += _root.LinkClickedEventHandler;
                    }
                    newItem.Action += ActionEventHandler;
                    FavoriteGroups.Add(newItem);
                    break;
                default:
                    break;
            }
        }

        internal void AddFavorite(object param)
        {
            int groupId = (int)param;
            Favorite favorite;
            switch (_root.ActivePanel)
            {
                case (int)Panels.feat:
                    if (_root.FeatsVM.SelectedFeat == null) return;
                    favorite = new Favorite(_root.FeatsVM.SelectedFeat);
                    if (!favorite.Save()) return;
                    break;
                default:
                    return;
            }
            Repository.SaveFavorite(favorite);
            FavoriteGroupViewModel currentGroup = FavoriteGroups.Single(item => item.Id == groupId);
            currentGroup.AddFavorite(favorite);
            currentGroup.Favorites.Last().LinkClicked += _root.LinkClickedEventHandler;
            //FavoriteGroupViewModel newItem = new FavoriteGroupViewModel(group);
        }
        #endregion
    }
}
