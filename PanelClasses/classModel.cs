using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Threading;

namespace AlphaCodex
{
    public partial class MainWindow : Window
    {
        /** properties **/

        //public List<ClassExtraField> classesExtraFieldsList { get; set; }
        public static ObservableCollection<Class> Classes  { get; set; }
        //public static ObservableCollection<featPrerequisite> classPrerequisitesList  { get; set; }

        #region initialization
        private void ClassesPanel_Initialized(object sender, System.EventArgs e)
        {
            //classesExtraFieldsList = new List<ClassExtraField>();
            Classes = new ObservableCollection<Class>();

            //getClassesExtraFields();
            GetClasses();
            System.Diagnostics.Debug.WriteLine("[Classes] List Initialized");
        }
        #endregion

        #region Database Access
        /* retrieve the list of classes from the DB*/
        private void GetClasses()
        {
            string queryString = "SELECT * FROM 'class' ORDER BY name";

            using (SQLiteConnection connDB = new SQLiteConnection(conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Classes.Add(new Class(reader));
                }
                reader.Close();
                connDB.Close();
            }
        }
        #endregion
    }
}