

using AlphaCodex.Models;
using System;
using System.Linq;

namespace AlphaCodex.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public enum Panels
        {
            player,
            feat
        }
        #region Fields
        private Panels _activePanel = Panels.player;
        private MainRepository _repository;
        private FeatPanelViewModel _featsVM;
        private PlayerPanelViewModel _playerVM;

        #endregion Fields

        #region Properties
        public int ActivePanel
        {
            get { return (int)_activePanel; }
            set {
                if (_activePanel == (Panels)value) return;
                _activePanel = (Panels)value;
                base.OnPropertyChanged("ActivePanel");
            }
        }
        public void SetActivePanel(Panels value)
        {
            if (_activePanel == value) return;
            _activePanel = value;
            base.OnPropertyChanged("ActivePanel");
        }

        public FeatPanelViewModel FeatsVM => _featsVM;
        public PlayerPanelViewModel PlayerVM => _playerVM;
        #endregion Properties

        #region Constructor
        public MainViewModel(MainRepository repository)
        {
            _repository = repository;
            _featsVM = new FeatPanelViewModel(repository.Feats, this);
            _playerVM = new PlayerPanelViewModel(repository.Player, this);
        }
        #endregion
        #region Methods
        public void LinkClickedEventHandler(object sender, EventArgs e)
        {
            Link link = e as Link;
            switch (link.Type)
            {
                case "feat":
                    SetActivePanel(Panels.feat);
                    _featsVM.SelectedFeat = _featsVM.Feats.Single(feat => feat.Id == link.ParentId);
                    break;
            }
        }
        #endregion
    }
}
