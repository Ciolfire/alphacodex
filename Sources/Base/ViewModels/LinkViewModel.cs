using AlphaCodex.Models;
using AlphaCodex.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlphaCodex.ViewModels
{
    public class LinkViewModel : ViewModelBase
    {
        #region Fields
        private Link _link;
        #endregion

        #region Properties
        public string Name
        {
            get { return _link.Name; }
            set
            {
                if (value == _link.Name) return;
                _link.Name = value;
                base.OnPropertyChanged("Name");
            }
        }

        public int ParentId
        {
            get { return _link.ParentId; }
            set
            {
                if (value == _link.ParentId) return;
                _link.ParentId = value;
                base.OnPropertyChanged("ParentId");
            }
        }
        public int LinkId
        {
            get { return _link.LinkId; }
            set
            {
                if (value == _link.LinkId) return;
                _link.LinkId = value;
                base.OnPropertyChanged("LinkId");
            }
        }
        #endregion

        #region Commands
        RelayCommand _navigateCommand;
        public ICommand NavigateCommand
        {
            get
            {
                if (_navigateCommand == null)
                {
                    _navigateCommand = new RelayCommand(param => this.Navigate());
                }
                return _navigateCommand;
            }
        }

        public void Navigate()
        {
            OnLinkClicked(_link);
        }
        #endregion

        #region Constructor
        public LinkViewModel(Link link)
        {
            _link = link ?? throw new ArgumentNullException("Link");
            DisplayName = link.Name;
        }

        public LinkViewModel(Favorite favorite)
        {
            if (favorite == null)
            {
                throw new ArgumentNullException("Link");
            }
            Link link = new Link(favorite.Name, favorite.Id, favorite.Type);
            if (string.IsNullOrWhiteSpace(favorite.Name))
                DisplayName = link.ParentId.ToString();
            else
                DisplayName = favorite.Name;
        }
        #endregion

        #region Events
        public event EventHandler LinkClicked;

        protected virtual void OnLinkClicked(EventArgs e)
        {
            LinkClicked?.Invoke(this, e);
        }
        #endregion
    }
}
