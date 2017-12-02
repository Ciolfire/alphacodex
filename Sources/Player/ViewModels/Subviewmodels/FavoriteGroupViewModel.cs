using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaCodex.ViewModels
{
    class FavoriteGroupViewModel : ViewModelBase
    {
        #region Fields
        private FavoriteGroup _favoriteGroup;
        private ObservableCollection<LinkViewModel> _favorites = new ObservableCollection<LinkViewModel>();
        #endregion Fields
        #region Properties
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
        #region Constructor
        public FavoriteGroupViewModel(FavoriteGroup favoriteGroup)
        {
            _favoriteGroup = favoriteGroup;
            if (favoriteGroup.Favorites.Any())
            {
                _favorites = new ObservableCollection<LinkViewModel>(favoriteGroup.Favorites.Select(favorite => new LinkViewModel(favorite)));
            }
        }
        #endregion Constructor
    }
}
