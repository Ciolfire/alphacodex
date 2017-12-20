using AlphaCodex.Data;
using AlphaCodex.Events;
using AlphaCodex.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AlphaCodex.ViewModels
{
    public class FavoriteGroupViewModel : ViewModelBase
    {
        #region Fields
        private FavoriteGroup _favoriteGroup;
        private ObservableCollection<LinkViewModel> _favorites = new ObservableCollection<LinkViewModel>();
        private bool _isNameChanged = false;
        #endregion Fields
        #region Properties
        public new string DisplayName
        {
            get { return base.DisplayName; }
            set
            {
                if (value == base.DisplayName) return;
                base.DisplayName = value;
                IsNameChanged = true;
                if (base.DisplayName == Name)
                    IsNameChanged = false;
                OnPropertyChanged("DisplayName");
            }

        }

        public string Name
        {
            get{ return _favoriteGroup.Name; }
            set
            {
                if (value == _favoriteGroup.Name) return;
                _favoriteGroup.Name = value;
                base.OnPropertyChanged("Name");
            }
        }
        public int Id
        {
            get { return _favoriteGroup.Id; }
            set
            {
                if (value == _favoriteGroup.Id) return;
                _favoriteGroup.Id = value;
                base.OnPropertyChanged("Id");
            }
        }

        public bool IsNameChanged
        {
            get { return _isNameChanged; }
            set
            {
                if (value == _isNameChanged) return;
                _isNameChanged = value;
                OnPropertyChanged("IsNameChanged");
            }
        }

        public ObservableCollection<LinkViewModel> Favorites
        {
            get { return _favorites; }
            set
            {
                if (value == _favorites) return;
                _favorites = value;
                base.OnPropertyChanged("Favorites");
            }
        }
        #endregion Properties
        #region Events
        public delegate void ActionEventHandler(object sender, ActionEventArgs args);

        public event ActionEventHandler Action;
        #endregion
        #region Commands
        RelayCommand _changeGroupNameCommand;
        public ICommand ChangeGroupNameCommand
        {
            get
            {
                if (_changeGroupNameCommand == null)
                {
                    _changeGroupNameCommand = new RelayCommand(param => ChangeGroupName());
                }
                return _changeGroupNameCommand;
            }
        }
        private void ChangeGroupName()
        {
            DatabaseAccess da = new DatabaseAccess();

            if (!da.UpdateFavoriteGroup(this._favoriteGroup, DisplayName)) return;
            string oldName = Name;
            Name = DisplayName;
            base.OnPropertyChanged("Name");
            IsNameChanged = false;
            MessageBox.Show("Name changed", oldName + " ➞ " + Name, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        RelayCommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(param => Delete());
                }
                return _deleteCommand;
            }
        }
        private void Delete()
        {
            if (MessageBox.Show("Do you really want to delete this group ?\nYou will not be able to recover it", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {

            }
            else
            {
                OnActionTaken("delete");
            }
        }

        RelayCommand _duplicateCommand;
        public ICommand DuplicateCommand
        {
            get
            {
                if (_duplicateCommand == null)
                {
                    _duplicateCommand = new RelayCommand(param => Duplicate());
                }
                return _duplicateCommand;
            }
        }

        private void Duplicate()
        {
            OnActionTaken("duplicate");
        }
        #endregion
        #region Constructor
        public FavoriteGroupViewModel(FavoriteGroup favoriteGroup)
        {
            _favoriteGroup = favoriteGroup;
            if (string.IsNullOrWhiteSpace(favoriteGroup.Name))
                base.DisplayName = "Default";
            else
                base.DisplayName = favoriteGroup.Name;
            if (favoriteGroup.Favorites.Any())
            {
                _favorites = new ObservableCollection<LinkViewModel>(favoriteGroup.Favorites.Select(favorite => new LinkViewModel(favorite)));
            }
        }

        public FavoriteGroupViewModel(FavoriteGroupViewModel group)
        {
            _favoriteGroup = group._favoriteGroup;
            _favorites = group.Favorites;
            _favoriteGroup.Name = Name + " - Copy ";
            base.DisplayName = Name;
        }
        #endregion Constructor

        #region Methods
        protected virtual void OnActionTaken(string action)
        {
            Action?.Invoke(this, new ActionEventArgs(action));
        }

        internal void AddFavorite(Favorite favorite)
        {
            _favoriteGroup.Add(favorite);
            _favorites.Add(new LinkViewModel(favorite));
        }
        #endregion
    }
}
