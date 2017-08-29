using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace AlphaCodex
{
    public class Feat : ObservableObject
    {
        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public short Type { get; set; }
        public short TypeExtra { get; set; }

        public string Prerequisite { get; set; }
        public string SimplePrerequisite { get; set; }
        public List<int> Children { get; set; }
        public List<int> Older { get; set; }
        public string Benefit { get; set; }
        public string Normal { get; set; }
        public string Special { get; set; }
        public string Note { get; set; }

        public bool Fighter { get; set; }
        public int Page { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

        public string Info { get; set; }
        public bool Current { get; set; }
        public int CurrentId { get; set; }

        public short SubtypeId { get; set; }
        public FeatSubtype Subtype { get; set; }

        public string TypePicture { get; set; }

        private Visibility _visibility = Visibility.Visible;
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                NotifyPropertyChanged("Visibility");
            }
        }
        #endregion

        #region Constructors
        public Feat() {
            this.TypePicture = "/AlphaCodex;component/data/pictures/UI/feats/type/none.png";
            this.SubtypeId = 0;
        }
        public Feat(SQLiteDataReader reader, ObservableCollection<FeatType> featTypeList, ObservableCollection<FeatSubtype> featSubtypesList, ObservableCollection<Book> books)
        {
            Id = (int)(long)reader["id"];

            Name = reader["name"] as string;
            Description = reader["description"] as string;

            Type = (short)(long)reader["type"];
            TypeExtra = (short)(long)reader["type_extra"];
            SubtypeId = (short)(long)reader["subtype"];

            Prerequisite = "" + reader["prerequisite"] as string;
            Benefit = "" + reader["benefit"] as string;
            Normal = "" + reader["normal"] as string;
            Special = "" + reader["special"] as string;
            Note = "" + reader["note"] as string;

            CurrentId = (int)(long)reader["current"];
            if (CurrentId != 0)
            {
                Current = false;
                Visibility = Visibility.Collapsed;
            }
            else
                Current = true;

            Fighter = (long)reader["fighter"] != 0;

            Page = (int)(long)reader["book_page"];
            BookId = (int)(long)reader["book_id"];
            Info = reader["info"] as string;
            GetTypePicture(featTypeList);
            GetSubtypePicture(featSubtypesList);
            GetBook(books);
        }
        #endregion

        #region Methods

        private void GetTypePicture(ObservableCollection<FeatType> featTypeList)
        {

            foreach (FeatType Ltype in featTypeList)
            {
                if (Type == Ltype.Id)
                {
                    if (Fighter == true && Type == 1)
                        this.TypePicture = "/AlphaCodex;component/data/pictures/UI/feats/type/warrior.png";
                    else
                        this.TypePicture = Ltype.Picture;
                    break;
                }
            }
        }

        private void GetSubtypePicture(ObservableCollection<FeatSubtype> featSubtypesList)
        {
            foreach (FeatSubtype Lsubtype in featSubtypesList)
            {
                if (SubtypeId == Lsubtype.Id)
                {
                    this.Subtype = Lsubtype;
                    break;
                }
            }
        }

        private void GetBook(ObservableCollection<Book> books)
        {
            foreach (Book book in books)
            {
                if (this.BookId == book.Id)
                {
                    this.Book = book;
                    break;
                }
            }
        }

        public void GenerateSimplePrerequisitesString()
        {
            string[] toSplit = Regex.Split(this.Prerequisite, @"({[0-9]+})");
            string result = "";
            int id;
            foreach (string part in toSplit)
            {
                // Check if it's a variable
                if (Regex.Match(part, @"({[0-9]+})") != Match.Empty)
                {
                    // Keep only the id
                    id = int.Parse(Regex.Replace(part, @"[{}]+", ""));
                    FeatPrerequisite prerequisite = MainWindow.FeatPrerequisites.Single(prereq => (prereq.ChildId == this.Id && prereq.LinkId == id));
                    result += MainWindow.Feats.Single(feat => feat.Id == prerequisite.ParentId).Name;
                }
                else
                {
                    result += part;
                }
                SimplePrerequisite = result;
            }
        }

        #endregion
    }
}
