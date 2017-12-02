using AlphaCodex.Models;
using AlphaCodex.Tools;
using System;
using System.Windows.Input;

namespace AlphaCodex.ViewModels
{
    public class FeatTypeViewModel : ViewModelBase
    {
        #region Fields
        private FeatType _type;
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
            get { return _type.Id; }
            set
            {
                if (value == _type.Id) return;
                _type.Id = value;
                base.OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return _type.Name; }
            set
            {
                if (value == _type.Name) return;
                _type.Name = value;
                base.OnPropertyChanged("Name");
            }
        }
        public string Picture
        {
            get { return _type.Picture; }
            set
            {
                if (value == _type.Picture) return;
                _type.Picture = value;
                base.OnPropertyChanged("Picture");
            }
        }
        public string Info
        {
            get { return _type.Info; }
            set
            {
                if (value == _type.Info) return;
                _type.Info = value;
                base.OnPropertyChanged("Info");
            }
        }
        #endregion
        #region Constructor
        public FeatTypeViewModel(string displayName, FeatType type)
        {
            this._type = type ?? throw new ArgumentNullException("Feat");
            DisplayName = displayName;
        }

        public FeatTypeViewModel(FeatType type)
        {
            this._type = type ?? throw new ArgumentNullException("Feat");
            DisplayName = type.Name;
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
