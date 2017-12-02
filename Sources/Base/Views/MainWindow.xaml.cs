using AlphaCodex.Data;
using AlphaCodex.Models;
using System.Windows;

namespace AlphaCodex
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        #region toremove
        //// generate the feats requirements from feats
        //private void generateFromFeats(Feat currentFeat, IOrderedEnumerable<Feat> linq)
        //{
        //    int linkId;
        //    // check if the feat already has saved parents, if yes, get the last index
        //    if (FeatPrerequisites.Any(x => x.ChildId == currentFeat.Id))
        //        linkId = FeatPrerequisites.Last(x => x.ChildId == currentFeat.Id).LinkId + 1;
        //    else
        //        linkId = 0;
        //    string prereq = currentFeat.Prerequisite;
        //    // Note: A child is a feat which HAS prerequisite, a parent is a feat NEEDED by another feat
        //    string queryInsert = "INSERT INTO feat_prerequisite (child_id, type, link_id, parent_id) VALUES (@child, @type, @link, @parent)";
        //    string queryUpdate = "UPDATE feat SET 'prerequisite' = @prerequisite WHERE feat.id = @id";

        //    foreach (Feat feat in linq)
        //    {
        //        // if the feat is in the list of prerequisite, it's a parent
        //        if (feat.Current)
        //        {
        //            if (prereq.Contains(feat.Name) && currentFeat.Id != feat.Id)
        //            {
        //                System.Diagnostics.Debug.WriteLine("[child] " + currentFeat.Name + " -> [parent] " + feat.Name);
        //                prereq = prereq.Replace(feat.Name, "{" + linkId + "}");
        //                // add the feat to the list of prerequisite with id_child = currentFeat.Id, id_parent = feat.Id, and type = 'feat'
        //                using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
        //                {
        //                    connDB.Open();
        //                    SQLiteCommand command = new SQLiteCommand(queryInsert, connDB);
        //                    command.Parameters.Add(new SQLiteParameter("@child", currentFeat.Id));
        //                    command.Parameters.Add(new SQLiteParameter("@type", "feat"));
        //                    command.Parameters.Add(new SQLiteParameter("@link", linkId));
        //                    command.Parameters.Add(new SQLiteParameter("@parent", feat.Id));

        //                    command.ExecuteNonQuery();
        //                    connDB.Close();
        //                }
        //                //MessageBox.Show("before: |" + old + "| after: |" + input + "|");
        //                linkId++;
        //            }
        //        }
        //    }
        //    if (prereq != currentFeat.Prerequisite)
        //    {
        //        using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
        //        {
        //            connDB.Open();
        //            SQLiteCommand command = new SQLiteCommand(queryUpdate, connDB);
        //            command.Parameters.Add(new SQLiteParameter("@id", currentFeat.Id));
        //            command.Parameters.Add(new SQLiteParameter("@prerequisite", prereq));
        //            command.ExecuteNonQuery();
        //            connDB.Close();
        //        }
        //    }
        //}

        // To be moved later
        // Used to create the prerequisite of the actual feats
        //private void CreatePrerequisiteBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    // prepare the data
        //    // the list of feat, ordered to avoid issues
        //    var linqFeats = Feats.OrderByDescending(x => x.Name.Length);
        //    // 
        //    System.Diagnostics.Debug.WriteLine("[CREATING PREREQUISITE] start");
        //    foreach (Feat feat in Feats)
        //    {
        //        // for each feat in the least, check if there are feat in the prerequisite
        //        generateFromFeats(feat, linqFeats);
        //        // the idea is to check every type of prerequisite, feats, race, skill, and so on
        //        // Only feat are implemented for now
        //    }
        //    System.Diagnostics.Debug.WriteLine("[CREATING PREREQUISITE] Done");

        //    // [IMPROVE] Should reload from database, to avoid duplicate entries etc.
        //    // Or maybe not, this is a dev feature only, maybe force quit application once completed.
        //    // [BUG] Still feat are observable but don't update as intended, to be corrected.
        //}

        #endregion
    }
}
