using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AlphaCodex
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region properties

        // colors
        Brush TitleBlue = (Brush)(new BrushConverter().ConvertFrom("#FF003364"));

        // database
        private static string path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Directory.GetCurrentDirectory()));
        public static string conn = ("Data Source=" + Path + @"\data\LyndisLibrary.sqlite;Version=3;");
        // test database
        //public static string conn = ("Data Source=" + path + @"\data\test.sqlite;Version=3;");

        List<string> data = new List<string>();
        // inside data
        private ObservableCollection<Book> _books = new ObservableCollection<Book>();
        private ObservableCollection<spellSchool> _spellSchoolList = new ObservableCollection<spellSchool>();
        private ObservableCollection<spellDescriptor> _spellDescriptorList = new ObservableCollection<spellDescriptor>();
        private ObservableCollection<domain> _domainList = new ObservableCollection<domain>();


        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                NotifyPropertyChanged("Books");
            }
        }
        public ObservableCollection<spellSchool> SpellSchoolList
        {
            get { return _spellSchoolList; }
            set
            {
                _spellSchoolList = value;
                NotifyPropertyChanged("SpellSchoolList");
            }
        }
        public ObservableCollection<spellDescriptor> spellDescriptorList
        {
            get { return _spellDescriptorList; }
            set
            {
                _spellDescriptorList = value;
                NotifyPropertyChanged("SpellDescriptorList");
            }
        }
        public ObservableCollection<domain> DomainList
        {
            get { return _domainList; }
            set
            {
                _domainList = value;
                NotifyPropertyChanged("DomainList");
            }
        }

        public static string Path { get => path; set => path = value; }

        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods

        public static BitmapSource CaptureScreen(Visual target, double dpiX, double dpiY)
        {
            if (target == null)
            {
                return null;
            }
            FrameworkElement elem = (FrameworkElement)target;
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)(elem.ActualWidth * dpiX / 96.0),
                                                            (int)(elem.ActualHeight * dpiY / 96.0),
                                                            dpiX,
                                                            dpiY,
                                                            PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(target);
                ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }
            rtb.Render(dv);
            return rtb;
        }

        /// <summary>
        /// Recording of the Event
        /// </summary>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Create a dataTable based on the columns 
        /// </summary>
        private DataTable CreateTable(List<Column> columns)
        {
            var table = new DataTable();
            // create the columns
            foreach (Column column in columns)
                table.Columns.Add(column.header);
            // create the rows
            List<string> rows = new List<string>();
            for (int i = 0; i < columns[0].rows.Count; i++)
            {
                rows.Clear();
                foreach (Column column in columns)
                {
                    rows.Add(column.rows.ElementAt(i));
                }
                table.Rows.Add(rows.ToArray());
            }
            return table;
        }

        /// <summary>
        /// Create the text style
        /// </summary>
        public void ParseText(string features, string parent, TextBlock target)
        {
            target.Inlines.Clear();
            if (features == null)
                return;
            Collection<string> tmpPre = new Collection<string>();

            string boldPattern = @"(\*{2}.+\*{2})";
            string italicPattern = @"(\~[^~]+\~)";
            string tablePattern = @"(\[{2}[0-9]+\]{2})";

            string[] toSplit = Regex.Split(features, boldPattern + "|" + italicPattern + "|" + tablePattern);
            foreach (string part in toSplit)
            {
                // Check if it's a headline to "bold" it
                if (Regex.IsMatch(part, boldPattern))
                    target.Inlines.Add(new Bold(new Run(part.Trim('*')) { Foreground = TitleBlue, FontSize = 16 }));
                // else check if it's italic
                else if (Regex.IsMatch(part, italicPattern))
                    target.Inlines.Add(new Italic(new Run(part.Trim('~')) { Foreground = TitleBlue }));
                // else check if it's a table
                else if (Regex.IsMatch(part, tablePattern))
                {
                    var grid = new DataGrid()
                    {
                        ItemsSource = FetchTable(parent, int.Parse(Regex.Replace(part, "[^0-9]", ""))).DefaultView,
                        Style = Application.Current.FindResource("tableStyle") as Style
                    };
                    target.Inlines.Add(grid);
                }
                // else normal text
                else
                    target.Inlines.Add(part);
            }
        }

        #endregion

        #region Get Data
        /// <summary>
        /// Retrieve the list of abilities from the DB
        /// </summary>
        private void GetAbilities()
        {
            Abilities = new List<Ability>();

            string queryString = "SELECT * FROM 'ability'";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Abilities.Add(new Ability(reader));
                }
                reader.Close();
                connDB.Close();
            }
            System.Diagnostics.Debug.WriteLine("[General] Abilities Initialized");
        }

        private void GetAlignments()
        {
            Alignments = new List<Alignment>();

            string queryString = "SELECT * FROM 'alignment'";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Alignments.Add(new Alignment(reader));
                }
                reader.Close();
                connDB.Close();
            }
            System.Diagnostics.Debug.WriteLine("[General] Alignments Initialized");
        }


        private void GetPlayer()
        {
            string queryString = "SELECT * FROM 'player' WHERE id LIKE 1";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();

                reader.Read();
                player = new Player(reader);
                reader.Close();
                connDB.Close();
            }
            System.Diagnostics.Debug.WriteLine("[General] Player Initialized");
        }

        private void GetSkills()
        {
            Skills = new List<Skill>();

            string queryString = "SELECT * FROM 'skill'";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Skills.Add(new Skill(reader));
                }
                reader.Close();
                connDB.Close();
            }
            System.Diagnostics.Debug.WriteLine("[General] Skills Initialized");
        }

        /// <summary>
        /// Retrieve the list of books from the DB
        /// </summary>
        private void GetBooks()
        {
            Books = new ObservableCollection<Book>();

            string queryString = "SELECT * FROM 'book'";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Books.Add(new Book(reader));
                }
                reader.Close();
                connDB.Close();
            }
            System.Diagnostics.Debug.WriteLine("[General] Books Initialized");
        }

        /// <summary>
        /// Retrieve the list of magic schools from the DB
        /// </summary>
        private void GetSpellSchoolList()
        {
            string queryString = "SELECT * FROM 'spell_school'";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    SpellSchoolList.Add(new spellSchool(reader, connDB));
                }
                reader.Close();
                connDB.Close();
            }
        }

        /// <summary>
        /// Retrieve the list of spell descriptors from the DB
        /// </summary>
        private void GetSpellDescriptorList()
        {
            string queryString = "SELECT * FROM 'spell_descriptor'";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    spellDescriptorList.Add(new spellDescriptor(reader));
                }
                reader.Close();
                connDB.Close();
            }
        }

        /// <summary>
        /// Retrieve the list of spellcaster classes from the DB
        /// </summary>
        private void GetSpellDomainList()
        {
            string queryString = "SELECT * FROM 'spell_domain'";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DomainList.Add(new domain(reader));
                }
                reader.Close();
                connDB.Close();
            }
        }

        /// <summary>
        /// fetch Table data from the DB
        /// </summary>
        public DataTable FetchTable(string name, int id)
        {
            List<Column> columns = new List<Column>();
            string queryString = "SELECT * FROM '" + name + "_table' WHERE table_id = " + id;
            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    columns.Add(new Column(reader));
                reader.Close();
                connDB.Close();
            }
            return CreateTable(columns);
        }
        #endregion
    }
}