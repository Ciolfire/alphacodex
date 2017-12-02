using AlphaCodex.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaCodex.Models
{
    class MainRepository
    {
        #region Fields
        private List<Book> _books;
        private FeatRepository _feats;
        private PlayerRepository _player;
        #endregion

        #region Properties
        public FeatRepository Feats => _feats;
        public PlayerRepository Player => _player;
        public List<Book> GetBooks() => _books;
        #endregion Properties

        #region Constructor
        public MainRepository()
        {
            DatabaseAccess da = new DatabaseAccess();
            _books = da.GetBooks();
            _feats = new FeatRepository();
            _player = new PlayerRepository();

            _player.SetFavoritesNames(this);
        }
        #endregion
        #region Methods
        #endregion
    }
}
