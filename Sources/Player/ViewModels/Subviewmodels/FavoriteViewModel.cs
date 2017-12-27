using AlphaCodex.Data;
using AlphaCodex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaCodex.ViewModels
{
    public class FavoriteViewModel : LinkViewModel
    {
        #region Fields
        //private new Favorite _link;
        #endregion
        #region Properties
        public bool IsSelected { get; set; }
        public Favorite Favorite { 
            get { return _link as Favorite; }
            set
            {
                if (value == _link) return;
                _link = value;
                base.OnPropertyChanged("Favorite");
            }
        }
        #endregion Properties

        #region Commands
        public override void Navigate()
        {
            base.OnLinkClicked(Favorite);
        }
        #endregion

        #region Constructor
        public FavoriteViewModel(Favorite favorite)
        {
            _link = favorite ?? throw new ArgumentNullException("Favorite");
            if (string.IsNullOrWhiteSpace(favorite.Name))
                DisplayName = Favorite.ParentId.ToString();
            else
                DisplayName = Favorite.Name;
            IsSelected = false;
        }
        #endregion Constructor
        #region Methods
        #endregion
    }
}
