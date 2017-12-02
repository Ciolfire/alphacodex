using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text.RegularExpressions;

namespace AlphaCodex.Models
{
    public class Feat
    {
        #region Fields
        private int _typeId;
        private int _typeExtraId;
        private int _subtypeId;
        private int _bookId;

        #endregion

        #region Properties
        public int Id { get; set; }
        public int CurrentId { get; set; }
        public int Page { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string PrerequisitesString { get; set; } = "";
        public string PrerequisitesBaseString { get; set; }
        public string Benefit { get; set; }
        public string Normal { get; set; }
        public string Special { get; set; }
        public string Note { get; set; }
        public string Info { get; set; }

        public bool IsFighter { get; set; }
        public bool IsCurrent { get; set; }

        public FeatType Type { get; set; }
        public FeatType TypeExtra { get; set; }
        public FeatSubtype Subtype { get; set; }
        public Book Book { get; set; }

        public List<Link> Prerequisites { get; set; }
        public List<Link> Children { get; set; } = new List<Link>();
        public List<Link> History { get; set; } = new List<Link>();
        public Link CurrentFeat { get; set; } = null;
        #endregion

        #region Constructor
        public Feat(SQLiteDataReader reader)
        {
            Id = (int)(long)reader["id"];
            CurrentId = (int)(long)reader["current"];
            Page = (int)(long)reader["book_page"];

            Name = reader["name"] as string;
            Description = reader["description"] as string;
            Benefit = reader["benefit"] as string;
            Normal = reader["normal"] as string;
            Special =reader["special"] as string;
            Note = reader["note"] as string;
            Info = reader["info"] as string;

            IsCurrent = CurrentId == 0;
            IsFighter = (long)reader["fighter"] != 0;

            _typeId = (int)(long)reader["type"];
            _typeExtraId = (int)(long)reader["type_extra"];
            _subtypeId = (int)(long)reader["subtype"];
            _bookId = (int)(long)reader["book_id"];
            PrerequisitesBaseString = reader["prerequisite"] as string;
        }
        #endregion

        #region Methods
        public bool SetType(List<FeatType> featTypeList)
        {
            if (_typeExtraId != 0)
                foreach (FeatType Ltype in featTypeList)
                {
                    if (_typeExtraId == Ltype.Id)
                    {
                        TypeExtra = Ltype;
                        break;
                    }
                }
            foreach (FeatType Ltype in featTypeList)
            {
                if (_typeId == Ltype.Id)
                {
                    Type = Ltype;
                    return true;
                }
            }
            System.Diagnostics.Debug.WriteLine("[Error] Missing type: " + _typeId.ToString());
            return false;
        }

        public bool SetSubtype(List<FeatSubtype> featSubtypesList)
        {
            foreach (FeatSubtype Lsubtype in featSubtypesList)
            {
                if (_subtypeId == Lsubtype.Id)
                {
                    Subtype = Lsubtype;
                    return true;
                }
            }
            System.Diagnostics.Debug.WriteLine("[Error] Missing subtype: " + _subtypeId.ToString());
            return false;
        }

        public void SetBook(List<Book> books)
        {
            foreach (Book book in books)
            {
                if (_bookId == book.Id)
                {
                    Book = book;
                    break;
                }
            }
        }

        public void GeneratePrerequisitesString()
        {
            if (string.IsNullOrWhiteSpace(PrerequisitesBaseString)) return;
            if (!Prerequisites.Any())
            {
                PrerequisitesString = PrerequisitesBaseString;
                return;
            }
            string[] toSplit = Regex.Split(PrerequisitesBaseString, @"({[0-9]+})");
            int id;
            foreach (string part in toSplit)
            {
                // Check if it's a variable
                if (Regex.Match(part, @"({[0-9]+})") != Match.Empty)
                {
                    // Keep only the id
                    id = int.Parse(Regex.Replace(part, @"[{}]+", ""));
                    PrerequisitesString += Prerequisites.Single(prereq => (prereq.LinkId == id)).Name;
                }
                else
                {
                    PrerequisitesString += part;
                }
            }
        }
        #endregion
    }
}
