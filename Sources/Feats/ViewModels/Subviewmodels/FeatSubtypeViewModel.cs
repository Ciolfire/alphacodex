using AlphaCodex.Models;
using AlphaCodex.Tools;
using System;
using System.Windows.Input;

namespace AlphaCodex.ViewModels
{
    public class FeatSubtypeViewModel : ViewModelBase
    {
        #region Fields
        FeatSubtype _subtype;
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
            get { return _subtype.Id; }
            set
            {
                if (value == _subtype.Id) return;
                _subtype.Id = value;
                base.OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return _subtype.Name; }
            set
            {
                if (value == _subtype.Name) return;
                _subtype.Name = value;
                base.OnPropertyChanged("Name");
            }
        }
        public string Picture
        {
            get { return _subtype.Picture; }
            set
            {
                if (value == _subtype.Picture) return;
                _subtype.Picture = value;
                base.OnPropertyChanged("Picture");
            }
        }
        #endregion
        #region Constructor
        public FeatSubtypeViewModel(string displayName, FeatSubtype subtype)
        {
            DisplayName = displayName;
            this._subtype = subtype ?? throw new ArgumentNullException("Feat");
        }
        public FeatSubtypeViewModel(FeatSubtype subtype)
        {
            DisplayName = subtype.Name;
            this._subtype = subtype ?? throw new ArgumentNullException("Feat");
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
