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

        public static ObservableCollection<FeatType> FeatTypes { get; set; }
        public static ObservableCollection<FeatSubtype> FeatSubtypes { get; set; }
        public static ObservableCollection<Feat> Feats { get; set; }
        public static ObservableCollection<FeatPrerequisite> FeatPrerequisites { get; set; }

        #region initialization
        private void FeatPanel_Initialized(object sender, System.EventArgs e)
        { 
            GetFeatsTypes();
            System.Diagnostics.Debug.WriteLine("[Feats] Types Initialized");
            GetFeatsSubtypes();
            System.Diagnostics.Debug.WriteLine("[Feats] Subtypes Initialized");
            GetFeats();
            System.Diagnostics.Debug.WriteLine("[Feats] List Initialized");
            GetFeatsPrerequisites();
            System.Diagnostics.Debug.WriteLine("[Feats] Prerequisites Initialized");
            Feat CurrentFeat = null;
            foreach (Feat feat in Feats)
            {
                // Generate a text only prerequisite string
                feat.GenerateSimplePrerequisitesString();
                // Generate the feat history
                if (feat.CurrentId != 0)
                {
                    CurrentFeat = Feats.Single(newFeat => newFeat.Id == feat.CurrentId);
                    if (CurrentFeat.Older == null)
                        CurrentFeat.Older = new List<int>();
                    CurrentFeat.Older.Add(feat.Id);
                }
            }
            GetFeatsChildrens();
            System.Diagnostics.Debug.WriteLine("[Feats] Children Initialized");
        }
        #endregion

        #region Database Access
        /* retrieve the list of types from the DB*/
        private void GetFeatsTypes()
        {
            FeatTypes = new ObservableCollection<FeatType>();
            string queryString = "SELECT * FROM 'feat_type' ORDER BY name";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    FeatTypes.Add(new FeatType(reader));
                }
                reader.Close();
                connDB.Close();
            }
        }

        /* Retrive the list of subtypes from the DB*/
        private void GetFeatsSubtypes()
        {
            FeatSubtypes = new ObservableCollection<FeatSubtype>();
            string queryString = "SELECT * FROM 'feat_subtype' ORDER BY name";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    FeatSubtypes.Add(new FeatSubtype(reader));
                }
                reader.Close();
                connDB.Close();
            }
        }

        /* Retrieve the list of feats from the DB*/
        private void GetFeats()
        {
            Feats = new ObservableCollection<Feat>();
            string queryString = "SELECT * FROM 'feat' ORDER BY name";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();
                if (Feats != null)
                    Feats.Clear();
                while (reader.Read())
                {
                    Feats.Add(new Feat(reader, FeatTypes, FeatSubtypes, Books));
                }
                reader.Close();
                connDB.Close();
            }
        }

        /* retrieve the list of prerequistes from the DB*/
        private void GetFeatsPrerequisites()
        {
            FeatPrerequisites = new ObservableCollection<FeatPrerequisite>();
            string queryString = "SELECT * FROM 'feat_prerequisite'";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    FeatPrerequisites.Add(new FeatPrerequisite(reader));
                }
                reader.Close();
                connDB.Close();
            }
        }

        /* retrieve the list of childrens from the prerequisites*/
        private void GetFeatsChildrens()
        {
            List<int> childrenId = new List<int>();
            foreach (Feat feat in Feats)
            {
                // Search the prerequisite table for feats(children) that have the current feat as parent
                var children = FeatPrerequisites.Where(prerequisite => prerequisite.ParentId == feat.Id);
                if (children != null)
                {
                    childrenId.Clear();
                    foreach (FeatPrerequisite child in children)
                    {
                        childrenId.Add(child.ChildId);
                    }
                    feat.Children = childrenId.ToList();
                }
            }
        }

        #endregion
    }
}