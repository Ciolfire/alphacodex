using AlphaCodex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        #endregion Properties
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
