

using AlphaCodex.Models;

namespace AlphaCodex.ViewModels
{
    class MainViewModel
    {
        #region Fields
        private MainRepository _repository;
        private FeatPanelViewModel _featsVM;
        private PlayerPanelViewModel _playerVM;

        #endregion Fields

        #region Properties
        public FeatPanelViewModel FeatsVM => _featsVM;
        public PlayerPanelViewModel PlayerVM => _playerVM;
        #endregion Properties

        #region Constructor
        public MainViewModel(MainRepository repository)
        {
            _repository = repository;
            _featsVM = new FeatPanelViewModel(repository.Feats);
            _playerVM = new PlayerPanelViewModel(repository.Player);
        }
        #endregion Constructor

    }
}
