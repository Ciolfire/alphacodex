using AlphaCodex.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Linq;
using AlphaCodex.Tools;
using System.Windows.Input;

namespace AlphaCodex.ViewModels
{
    public class FeatPanelViewModel : ViewModelBase
    {
        public enum FeatSearch
        {
            Name,
            Benefit,
            Prerequisite
        }
        #region Fields
        private MainViewModel _root;
        private FeatRepository _featRepository;
        private FeatViewModel _feat = null;
        private ObservableCollection<FeatViewModel> _feats;
        private ObservableCollection<FeatTypeViewModel> _types;
        private ObservableCollection<FeatSubtypeViewModel> _subtypes;
        private ObservableCollection<BookViewModel> _books;

        private string _toFind;
        private FeatSearch _searchMode = FeatSearch.Name;
        #endregion
        #region Properties
        public FeatViewModel SelectedFeat
        {
            get
            {
                return _feat;
            }
            set
            {
                if (_feat != value)
                {
                    if (_feat != null)
                    {
                        _feat.IsSelected = false;
                        foreach (LinkViewModel link in _feat.Prerequisites)
                        {
                            link.LinkClicked -= LinkClickedEventHandler;
                        }
                        foreach (LinkViewModel link in _feat.Children)
                        {
                            link.LinkClicked -= LinkClickedEventHandler;
                        }
                        foreach (LinkViewModel link in _feat.History)
                        {
                            link.LinkClicked -= LinkClickedEventHandler;
                        }
                        if (_feat.CurrentFeat != null)
                        {
                            _feat.CurrentFeat.LinkClicked -= LinkClickedEventHandler;
                        }
                    }
                    _feat = value;
                    foreach (LinkViewModel link in _feat.Prerequisites)
                    {
                        link.LinkClicked += LinkClickedEventHandler;
                    }
                    foreach (LinkViewModel link in _feat.Children)
                    {
                        link.LinkClicked += LinkClickedEventHandler;
                    }
                    foreach (LinkViewModel link in _feat.History)
                    {
                        link.LinkClicked += LinkClickedEventHandler;
                    }
                    if (_feat.CurrentFeat != null)
                    {
                        _feat.CurrentFeat.LinkClicked += LinkClickedEventHandler;
                    }
                    OnPropertyChanged("SelectedFeat");
                }
            }
        }
        public ObservableCollection<FeatViewModel> Feats
        {
            get
            {
                if (_feats == null)
                {
                    _feats = new ObservableCollection<FeatViewModel>();
                }
                return _feats;
            }
        }
        public ObservableCollection<FeatTypeViewModel> Types
        {
            get
            {
                if (_types == null)
                {
                    _types = new ObservableCollection<FeatTypeViewModel>();
                }
                return _types;
            }
        }
        public ObservableCollection<FeatSubtypeViewModel> Subtypes
        {
            get
            {
                if (_subtypes == null)
                {
                    _subtypes = new ObservableCollection<FeatSubtypeViewModel>();
                }
                return _subtypes;
            }
        }
        public ObservableCollection<BookViewModel> Books
        {
            get
            {
                if (_books == null)
                {
                    _books = new ObservableCollection<BookViewModel>();
                }
                return _books;
            }
        }

        public string ToFind
        {
            get { return _toFind; }
            set
            {
                if (_toFind != value)
                {
                    base.OnPropertyChanged("ToFind");
                    _toFind = value;
                    SearchUpdated(value);
                }
            }
        }
        public FeatSearch SearchMode
        {
            get { return _searchMode; }
            set
            {
                if (_searchMode == value) return;
                _searchMode = value;
                SearchUpdated(ToFind);
                base.OnPropertyChanged("SearchMode");
            }
        }
        #endregion
        #region Commands
        RelayCommand _activateAllTypesCommand;
        public ICommand ActivateAllTypesCommand
        {
            get
            {
                if (_activateAllTypesCommand == null)
                {
                    _activateAllTypesCommand = new RelayCommand(param => this.ActivateAllTypes());
                }
                return _activateAllTypesCommand;
            }
        }
        private void ActivateAllTypes()
        {
            foreach (FeatTypeViewModel type in Types)
            {
                type.IsDisabled = false;
            }
        }

        RelayCommand _deactivateAllTypesCommand;
        public ICommand DeactivateAllTypesCommand
        {
            get
            {
                if (_deactivateAllTypesCommand == null)
                {
                    _deactivateAllTypesCommand = new RelayCommand(param => this.DeactivateAllTypes());
                }
                return _deactivateAllTypesCommand;
            }
        }
        private void DeactivateAllTypes()
        {
            foreach (FeatTypeViewModel type in Types)
            {
                type.IsDisabled = true;
            }
        }

        RelayCommand _activateAllSubtypesCommand;
        public ICommand ActivateAllSubtypesCommand
        {
            get
            {
                if (_activateAllSubtypesCommand == null)
                {
                    _activateAllSubtypesCommand = new RelayCommand(param => ActivateAllSubtypes());
                }
                return _activateAllSubtypesCommand;
            }
        }
        private void ActivateAllSubtypes()
        {
            foreach (FeatSubtypeViewModel subtype in Subtypes)
            {
                subtype.IsDisabled = false;
            }
        }

        RelayCommand _deactivateAllSubtypesCommand;
        public ICommand DeactivateAllSubtypesCommand
        {
            get
            {
                if (_deactivateAllSubtypesCommand == null)
                {
                    _deactivateAllSubtypesCommand = new RelayCommand(param => DeactivateAllSubtypes());
                }
                return _deactivateAllSubtypesCommand;
            }
        }
        private void DeactivateAllSubtypes()
        {
            foreach (FeatSubtypeViewModel subtype in Subtypes)
            {
                subtype.IsDisabled = true;
            }
        }

        RelayCommand _activateAllBooksCommand;
        public ICommand ActivateAllBooksCommand
        {
            get
            {
                if (_activateAllBooksCommand == null)
                {
                    _activateAllBooksCommand = new RelayCommand(param => ActivateAllBooks());
                }
                return _activateAllBooksCommand;
            }
        }
        private void ActivateAllBooks()
        {
            foreach (BookViewModel book in Books)
            {
                book.IsDisabled = false;
            }
        }

        RelayCommand _deactivateAllBooksCommand;
        public ICommand DeactivateAllBooksCommand
        {
            get
            {
                if (_deactivateAllBooksCommand == null)
                {
                    _deactivateAllBooksCommand = new RelayCommand(param => DeactivateAllBooks());
                }
                return _deactivateAllBooksCommand;
            }
        }
        private void DeactivateAllBooks()
        {
            foreach (BookViewModel book in Books)
            {
                book.IsDisabled = true;
            }
        }

        RelayCommand _selectSpecificBooksCommand;
        public ICommand SelectSpecificBooksCommand
        {
            get
            {
                if (_selectSpecificBooksCommand == null)
                {
                    _selectSpecificBooksCommand = new RelayCommand(param => SelectSpecificBooks(param));
                }
                return _selectSpecificBooksCommand;
            }
        }
        private void SelectSpecificBooks(object parameter)
        {
            string setting = parameter as string;
            foreach (BookViewModel book in Books)
            {
                if (book.Setting == setting)
                {
                    book.IsDisabled = false;
                } else if (setting == "core")
                {
                    book.IsDisabled = true;
                }
            }
        }

        RelayCommand _copyAsTextCommand;
        public ICommand CopyAsTextCommand
        {
            get
            {
                if (_copyAsTextCommand == null)
                {
                    _copyAsTextCommand = new RelayCommand(param => CopyAsText());
                }
                return _copyAsTextCommand;
            }
        }
        private void CopyAsText()
        {
            if (_feat == null)
                return;
            Clipboard.SetText(_feat.ToString());
        }
        #endregion
        #region Constructor
        public FeatPanelViewModel(FeatRepository feats, MainViewModel root)
        {
            _root = root;
            _featRepository = feats;
            //_feats = new ObservableCollection<FeatViewModel>(_featRepository.GetFeats().Select(feat => new FeatViewModel(feat)));
            foreach (Book book in feats.GetBooks())
            {
                BookViewModel newItem = new BookViewModel(book);
                newItem.PropertyChanged += BookPropertyChangedEventHandler;
                Books.Add(newItem);
            }
            foreach (Feat feat in _featRepository.GetFeats())
            {
                FeatViewModel newItem = new FeatViewModel(feat);
                newItem.PropertyChanged += FeatPropertyChangedEventHandler;
                Feats.Add(newItem);

            }
            foreach (FeatType type in _featRepository.GetTypes())
            {
                FeatTypeViewModel newItem = new FeatTypeViewModel(type);
                newItem.PropertyChanged += TypePropertyChangedEventHandler;
                Types.Add(newItem);
            }
            foreach (FeatSubtype subtype in _featRepository.GetSubtypes())
            {
                FeatSubtypeViewModel newItem = new FeatSubtypeViewModel(subtype);
                newItem.PropertyChanged += SubtypePropertyChangedEventHandler;
                Subtypes.Add(newItem);
            }
            SearchMode = FeatSearch.Name;
        }

        #endregion
        #region Methods

        private void SearchUpdated(string value)
        {
            bool contains;
            foreach (FeatViewModel feat in Feats)
            {
                switch (SearchMode)
                {
                    case FeatSearch.Name:
                        contains = feat.Name.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
                        break;
                    case FeatSearch.Benefit:
                        contains = feat.Benefit.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
                        break;
                    case FeatSearch.Prerequisite:
                        if (String.IsNullOrWhiteSpace(feat.PrerequisitesString) && !String.IsNullOrWhiteSpace(value))
                        {
                            contains = false;
                        }
                        else
                        {
                            contains = feat.PrerequisitesString.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
                        }
                        break;
                    default:
                        contains = false;
                        break;
                }
                feat.FitSearch = contains;
            }
        }

        private void LinkClickedEventHandler(object sender, EventArgs e)
        {
            Link link = e as Link;
            switch (link.Type)
            {
                case "feat":
                    SelectedFeat = _feats.Single(feat => feat.Id == link.ParentId);
                    break;
            }
        }

        private void FeatPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            FeatViewModel feat = (FeatViewModel)sender;
            switch (e.PropertyName)
            {
                case "IsSelected":
                    if (feat.IsSelected)
                    {
                        SelectedFeat = feat;
                    }
                    break;
            }
        }

        private void TypePropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            FeatTypeViewModel type = (FeatTypeViewModel)sender;
            switch (e.PropertyName)
            {
                case "IsDisabled":
                    foreach (FeatViewModel feat in Feats)
                    {
                        // NEED TO TAKE THE EXTRA TYPE INTO ACCOUNT
                        if (feat.Type.Id == type.Id)
                        {
                            feat.TypeIsActivated = !type.IsDisabled;
                        }
                    }
                    break;
            }
        }

        private void SubtypePropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            FeatSubtypeViewModel subtype = (FeatSubtypeViewModel)sender;
            switch (e.PropertyName)
            {
                case "IsDisabled":
                    foreach (FeatViewModel feat in Feats)
                    {
                        if (feat.Subtype.Id == subtype.Id)
                        {
                            feat.SubtypeIsActivated = !subtype.IsDisabled;
                        }
                    }
                    break;
            }
        }

        private void BookPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            foreach (FeatViewModel feat in Feats)
            {
                BookViewModel book = (BookViewModel)sender;
                switch (e.PropertyName)
                {
                    case "IsDisabled":
                        if (feat.Book.Id == book.Id)
                        {
                            feat.BookIsActivated = !book.IsDisabled;
                        }
                        break;
                }
            }
        }
        #endregion
    }
}
