using AlphaCodex.Data;
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
    public class PlayerViewModel : ViewModelBase
    {
        #region Fields
        private Player _player;
        #endregion Fields
        #region Properties
        public string Name
        {
            get { return _player.Name; }
            set
            {
                if (value == _player.Name) return;
                _player.Name = value;
                base.OnPropertyChanged("Name");
            }
        }
        #endregion
        #region Methods
        RelayCommand _saveNameCommand;
        public ICommand SaveNameCommand
        {
            get
            {
                if (_saveNameCommand == null)
                {
                    _saveNameCommand = new RelayCommand(param => SaveName(param));
                }
                return _saveNameCommand;
            }
        }
        private void SaveName(object param)
        {
            string text = param as string;
            DatabaseAccess da = new DatabaseAccess();
            if (!da.SavePlayer(text)) return;
            Name = text;
        }
        #endregion
        #region Constructor
        public PlayerViewModel(Player player)
        {
            _player = player;
        }
        #endregion Constructor
        #region Methods
        #endregion Methods
    }
}
