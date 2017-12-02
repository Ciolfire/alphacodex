using AlphaCodex.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AlphaCodex.Models
{
    public class FeatRepository
    {
        #region Fields
        private List<Feat> _feats;
        private List<FeatType> _types;
        private List<FeatSubtype> _subtypes;
        private List<Book> _books;
        #endregion

        #region Properties
        public List<Feat> GetFeats()
        {
            return _feats;
        }
        public List<FeatType> GetTypes()
        {
            return _types;
        }
        public List<FeatSubtype> GetSubtypes()
        {
            return _subtypes;
        }
        public List<Book> GetBooks()
        {
            return _books;
        }
        #endregion

        #region Events
        protected virtual void OnFeatAdded(EventArgs e)
        {
            FeatAdded?.Invoke(this, e);
        }

        public event EventHandler FeatAdded;
        #endregion

        #region Constructor
        public FeatRepository()
        {
            DatabaseAccess da = new DatabaseAccess();

            _feats = da.GetFeats();
            _types = da.GetFeatType();
            _subtypes = da.GetFeatSubtype();
            _books = da.GetBooks("feat");
            da.GetFeatsPrerequisites(_feats);
            if (_feats?.Any() == true)
            {
                InitializeTypes();
                InitializeSubtypes();
                InitializeBooks();
                InitializePrerequisites();
                GenerateHistory();
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Call the SetType function for each feat in the List 
        /// </summary>
        private void InitializeTypes()
        {
            var start = DateTime.Now;
            if (_types.Any() == true)
            {
                foreach (Feat feat in _feats)
                {
                    feat.SetType(_types);
                }
                System.Diagnostics.Debug.WriteLine("[Feat] Types applied in {0} ms.", (DateTime.Now - start).TotalMilliseconds);
            }
        }

        /// <summary>
        /// Call the SetSubtype function for each feat in the List 
        /// </summary>
        private void InitializeSubtypes()
        {
            var start = DateTime.Now;
            if (_subtypes.Any() == true)
            {
                foreach (Feat feat in _feats)
                {
                    feat.SetSubtype(_subtypes);
                }
                System.Diagnostics.Debug.WriteLine("[Feat] Subtypes applied in {0} ms.", (DateTime.Now - start).TotalMilliseconds);
            }
        }

        /// <summary>
        /// Call the SetBook function for each feat in the List 
        /// </summary>
        /// <param name="books"></param>
        private void InitializeBooks()
        {
            var start = DateTime.Now;
            if (_books.Any() == true)
            {
                foreach (Feat feat in _feats)
                {
                    feat.SetBook(_books);
                }
                System.Diagnostics.Debug.WriteLine("[Feat] Books applied in {0} ms.", (DateTime.Now - start).TotalMilliseconds);
            }
        }

        /// <summary>
        /// Set the name of the prerequisites
        /// </summary>
        private void InitializePrerequisites()
        {
            var start = DateTime.Now;
            foreach (Feat feat in _feats)
            {
                if (feat.Prerequisites.Any() == true)
                {
                    Feat featFound;
                    foreach (Link prerequisite in feat.Prerequisites)
                    {
                        if (prerequisite.Type == "feat")
                        {
                            featFound = _feats.Single(item => item.Id == prerequisite.ParentId);
                            if (featFound.Children == null)
                            {
                                featFound.Children = new List<Link>();
                            }
                            featFound.Children.Add(new Link(feat.Name, feat.Id, "feat"));
                            prerequisite.Name = featFound.Name;
                        }
                    }
                }
                feat.GeneratePrerequisitesString();
            }
            System.Diagnostics.Debug.WriteLine("[Feat] Prerequisites applied in {0} ms.", (DateTime.Now - start).TotalMilliseconds);
        }

        /// <summary>
        /// Generate the history for the feats that have been updated
        /// </summary>
        private void GenerateHistory()
        {
            var start = DateTime.Now;
            Feat featFound;
            foreach (Feat feat in _feats)
            {
                // To create the history, we start from the feats that are tagged as old
                if (!feat.IsCurrent)
                {
                    // We then find the parent
                    featFound = _feats.Single(item => item.Id == feat.CurrentId);
                    if (featFound.History == null)
                    {
                        featFound.History = new List<Link>();
                    }
                    // And add the old feat to the history of the current one
                    featFound.History.Add(new Link(feat.Book.Name, feat.Id, "feat"));
                    feat.CurrentFeat = new Link(featFound.Book.Name, featFound.Id, "feat");
                }
            }
            System.Diagnostics.Debug.WriteLine("[Feat] History generated in {0} ms.", (DateTime.Now - start).TotalMilliseconds);
        }

        //public void AddFeat(Feat feat)
        //{
        //    _feats.Add(feat);
        //    OnFeatAdded(new EventArgs());
        //}

        //public bool ContainsFeat(Feat feat)
        //{
        //    return _feats.Contains(feat);
        //}
        #endregion
    }
}
