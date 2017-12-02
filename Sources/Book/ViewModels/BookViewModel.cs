using AlphaCodex.Models;
using AlphaCodex.Tools;
using System;
using System.Windows.Input;

namespace AlphaCodex.ViewModels
{
    public class BookViewModel : ViewModelBase
    {
        #region Fields
        Book _book;
        private bool _isDisabled = false;
        #endregion
        #region Properties
        public bool IsDisabled
        {
            get { return _isDisabled; }
            set
            {
                if (value == _isDisabled) return;
                _isDisabled = value;
                base.OnPropertyChanged("IsDisabled");
            }
        }
        public int Id
        {
            get { return _book.Id; }
            set
            {
                if (value == _book.Id) return;
                _book.Id = value;
                base.OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return _book.Name; }
            set
            {
                if (value == _book.Name) return;
                _book.Name = value;
                base.OnPropertyChanged("Name");
            }
        }
        public string Setting
        {
            get { return _book.Setting; }
            set
            {
                if (value == _book.Setting) return;
                _book.Setting = value;
                base.OnPropertyChanged("Setting");
            }
        }
        public string Cover
        {
            get { return _book.Cover; }
            set
            {
                if (value == _book.Cover) return;
                _book.Cover = value;
                base.OnPropertyChanged("Cover");
            }
        }
        #endregion
        #region Constructor
        public BookViewModel(string displayName, Book book)
        {
            DisplayName = displayName;
            this._book = book ?? throw new ArgumentNullException("Book");
        }

        public BookViewModel(Book book)
        {
            DisplayName = book.Name;
            this._book = book ?? throw new ArgumentNullException("Book");
        }
        #endregion
        #region Commands
        RelayCommand _switchActivationCommand;
        public ICommand SwitchActivationCommand
        {
            get
            {
                if (_switchActivationCommand == null)
                {
                    _switchActivationCommand = new RelayCommand(param => this.SwitchActivation());
                }
                return _switchActivationCommand;
            }
        }
        #endregion
        #region Methods
        private void SwitchActivation()
        {
            IsDisabled = !IsDisabled;
        }
        #endregion
    }
}