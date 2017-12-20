using AlphaCodex.Models;
using AlphaCodex.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AlphaCodex.ViewModels
{
    public class FeatViewModel : ViewModelBase
    {
        #region Fields
        private Feat _feat;
        private bool _SubtypeIsActivated = true;
        private bool _TypeIsActivated = true;
        private bool _TypeExtraIsActivated = false;
        private bool _BookIsActivated = true;
        private bool _FitSearch = true;
        private bool _isSelected = false;
        private Visibility _visibility;

        private ObservableCollection<LinkViewModel> _prerequisites = new ObservableCollection<LinkViewModel>();
        private ObservableCollection<LinkViewModel> _children = new ObservableCollection<LinkViewModel>();
        private ObservableCollection<LinkViewModel> _history = new ObservableCollection<LinkViewModel>();
        //private bool _featTypeExtraIsActivated;
        #endregion
        #region Properties
        public int Id
        {
            get { return _feat.Id; }
            set
            {
                if (value == _feat.Id) return;
                _feat.Id = value;
                base.OnPropertyChanged("Id");
            }
        }
        public int Page
        {
            get { return _feat.Page; }
            set
            {
                if (value == _feat.Page) return;
                _feat.Page = value;
                base.OnPropertyChanged("Page");
            }
        }
        public string Name
        {
            get { return _feat.Name; }
            set
            {
                if (value == _feat.Name) return;
                _feat.Name = value;
                base.OnPropertyChanged("Name");
            }
        }
        public string Description
        {
            get { return _feat.Description; }
            set
            {
                if (value == _feat.Description) return;
                _feat.Description = value;
                base.OnPropertyChanged("Description");
            }
        }
        public string PrerequisitesString
        {
            get { return _feat.PrerequisitesString; }
            set
            {
                if (value == _feat.PrerequisitesString) return;
                _feat.PrerequisitesString = value;
                base.OnPropertyChanged("PrerequisitesString");
            }
        }
        public string PrerequisitesBaseString
        {
            get { return _feat.PrerequisitesBaseString; }
            set
            {
                if (value == _feat.PrerequisitesBaseString) return;
                _feat.PrerequisitesBaseString = value;
                base.OnPropertyChanged("PrerequisitesBaseString");
            }
        }
        public string Benefit
        {
            get { return _feat.Benefit; }
            set
            {
                if (value == _feat.Benefit) return;
                _feat.Benefit = value;
                base.OnPropertyChanged("Benefit");
            }
        }
        public string Normal
        {
            get { return _feat.Normal; }
            set
            {
                if (value == _feat.Normal) return;
                _feat.Normal = value;
                base.OnPropertyChanged("Normal");
            }
        }
        public string Special
        {
            get { return _feat.Special; }
            set
            {
                if (value == _feat.Special) return;
                _feat.Special = value;
                base.OnPropertyChanged("Special");
            }
        }
        public string Note
        {
            get { return _feat.Note; }
            set
            {
                if (value == _feat.Note) return;
                _feat.Note = value;
                base.OnPropertyChanged("Note");
            }
        }
        public string Info
        {
            get { return _feat.Info; }
            set
            {
                if (value == _feat.Info) return;
                _feat.Info = value;
                base.OnPropertyChanged("Info");
            }
        }

        public bool IsCurrent
        {
            get { return _feat.IsCurrent; }
            set
            {
                if (value == _feat.IsCurrent) return;
                _feat.IsCurrent = value;
                base.OnPropertyChanged("IsCurrent");
            }
        }
        public FeatType Type
        {
            get { return _feat.Type; }
            set
            {
                if (value == _feat.Type) return;
                _feat.Type = value;
                base.OnPropertyChanged("Type");
            }
        }
        public FeatType TypeExtra
        {
            get { return _feat.TypeExtra; }
            set
            {
                if (value == _feat.TypeExtra) return;
                _feat.TypeExtra = value;
                base.OnPropertyChanged("TypeExtra");
            }
        }
        public FeatSubtype Subtype
        {
            get { return _feat.Subtype; }
            set
            {
                if (value == _feat.Subtype) return;
                _feat.Subtype = value;
                base.OnPropertyChanged("Subtype");
            }
        }
        public Book Book
        {
            get { return _feat.Book; }
            set
            {
                if (value == _feat.Book) return;
                _feat.Book = value;
                base.OnPropertyChanged("Book");
            }
        }

        public LinkViewModel CurrentFeat;
        public ObservableCollection<LinkViewModel> Prerequisites
        {
            get { return _prerequisites; }
            set
            {
                if (value == _prerequisites) return;
                _prerequisites = value;
                base.OnPropertyChanged("Prerequisites");
            }
        }
        public ObservableCollection<LinkViewModel> Children
        {
            get { return _children; }
            set
            {
                if (value == _children) return;
                _children = value;
                base.OnPropertyChanged("Children");
            }
        }
        public ObservableCollection<LinkViewModel> History
        {
            get { return _history; }
            set
            {
                if (value == _history) return;
                _history = value;
                base.OnPropertyChanged("History");
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                base.OnPropertyChanged("IsSelected");
            }
        }
        public bool FitSearch
        {
            get
            { return _FitSearch; }
            set
            {
                if (value == _FitSearch) return;
                _FitSearch = value;
                SetVisibility();
            }
        }
        public bool TypeIsActivated
        {
            get
            { return _TypeIsActivated; }
            set
            {
                if (value == _TypeIsActivated) return;
                _TypeIsActivated = value;
                SetVisibility();
            }
        }
        public bool TypeExtraIsActivated
        {
            get
            { return _TypeExtraIsActivated; }
            set
            {
                if (value == _TypeExtraIsActivated) return;
                _TypeExtraIsActivated = value;
                SetVisibility();
            }
        }
        public bool BookIsActivated
        {
            get { return _BookIsActivated; }
            set
            {
                if (value == _BookIsActivated) return;
                _BookIsActivated = value;
                SetVisibility();
            }
        }
        public bool SubtypeIsActivated
        {
            get { return _SubtypeIsActivated; }
            set
            {
                if (value == _SubtypeIsActivated) return;
                _SubtypeIsActivated = value;
                SetVisibility();
            }
        }

        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                if (value == _visibility) return;
                _visibility = value;
                base.OnPropertyChanged("Visibility");
            }
        }
        #endregion
        #region Commands
        RelayCommand _selectCommand;
        public ICommand SelectCommand
        {
            get
            {
                if (_selectCommand == null)
                {
                    _selectCommand = new RelayCommand(param => this.Select());
                }
                return _selectCommand;
            }
        }

        public void Select()
        {
            if (!IsSelected)
            {
                IsSelected = true;
            }
        }
        #endregion

        #region Constructor
        public FeatViewModel(Feat feat)
        {
            this._feat = feat ?? throw new ArgumentNullException("Feat");
            if (feat.Prerequisites.Any())
            {
                Prerequisites = new ObservableCollection<LinkViewModel>(_feat.Prerequisites.Select(link => new LinkViewModel(link)));
            }
            if (feat.Children.Any())
            {
                Children = new ObservableCollection<LinkViewModel>(_feat.Children.Select(link => new LinkViewModel(link)));
            }
            if (feat.History.Any())
            {
                History = new ObservableCollection<LinkViewModel>(_feat.History.Select(link => new LinkViewModel(link)));
            }
            if (feat.CurrentFeat != null)
            {
                CurrentFeat = new LinkViewModel(feat.CurrentFeat);
            }
            if (feat.TypeExtra != null)
                TypeExtraIsActivated = true;
            SetDisplayName();
            SetVisibility();
        }
        #endregion

        #region Methods
        private void SetDisplayName()
        {
            DisplayName = _feat.Name;
            DisplayName += " [" + _feat.Type.Name;
            if (_feat.TypeExtra != null)
            {
                DisplayName += ", " + _feat.TypeExtra.Name;
            }
            DisplayName += "]";
        }

        private void SetVisibility()
        {
            if (IsCurrent
                        && (TypeIsActivated || TypeExtraIsActivated)
                        && SubtypeIsActivated
                        && FitSearch
                        && BookIsActivated)
                Visibility = Visibility.Visible;
            else
                Visibility = Visibility.Collapsed;
        }

        public override string ToString() => _feat.ToString();
        #endregion
    }
}
