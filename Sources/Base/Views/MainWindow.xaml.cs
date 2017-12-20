using AlphaCodex.Data;
using AlphaCodex.Models;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
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
        //    if (currentFeat.Prerequisites.Any())
        //        linkId = currentFeat.Prerequisites.Last().LinkId + 1;
        //    else
        //        linkId = 0;
        //    string prereq = currentFeat.PrerequisitesBaseString;
        //    // Note: A child is a feat which HAS prerequisite, a parent is a feat NEEDED by another feat
        //    string queryInsert = "INSERT INTO feat_prerequisite (child_id, type, link_id, parent_id) VALUES (@child, @type, @link, @parent)";
        //    string queryUpdate = "UPDATE feat SET 'prerequisite' = @prerequisite WHERE feat.id = @id";

        //    foreach (Feat feat in linq)
        //    {
        //        // if the feat is in the list of prerequisite, it's a parent
        //        if (feat.IsCurrent)
        //        {
        //            if (prereq.Contains(feat.Name) && currentFeat.Id != feat.Id)
        //            {
        //                var parts = prereq.Split(',');
        //                foreach (var part in parts)
        //                {
        //                    var curPart = part.Trim();
        //                    if (curPart.StartsWith(feat.Name) && (curPart.EndsWith(feat.Name) || curPart.EndsWith(feat.Name + ".")) ||
        //                        (curPart.StartsWith(feat.Name + "(") && curPart.EndsWith(")")) ||
        //                        curPart.Contains(feat.Name + " with the specified weapon") && feat.Name == "Weapon Focus")
        //                    {
        //                        System.Diagnostics.Debug.WriteLine("[child] " + currentFeat.Name + " -> [parent] " + feat.Name);
        //                        prereq = prereq.Replace(feat.Name, "{" + linkId + "}");
        //                        // add the feat to the list of prerequisite with id_child = currentFeat.Id, id_parent = feat.Id, and type = 'feat'
        //                        string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
        //                        using (SQLiteConnection connDB = new SQLiteConnection("Data Source=" + path + @"\Data\LyndisLibrary.sqlite;Version=3;"))
        //                        {
        //                            connDB.Open();
        //                            SQLiteCommand command = new SQLiteCommand(queryInsert, connDB);
        //                            command.Parameters.Add(new SQLiteParameter("@child", currentFeat.Id));
        //                            command.Parameters.Add(new SQLiteParameter("@type", "feat"));
        //                            command.Parameters.Add(new SQLiteParameter("@link", linkId));
        //                            command.Parameters.Add(new SQLiteParameter("@parent", feat.Id));

        //                            command.ExecuteNonQuery();
        //                            connDB.Close();
        //                        }
        //                    }
        //                }
        //                //MessageBox.Show("before: |" + old + "| after: |" + input + "|");
        //                linkId++;
        //            }
        //        }
        //    }
        //    if (prereq != currentFeat.PrerequisitesBaseString)
        //    {
        //        string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
        //        using (SQLiteConnection connDB = new SQLiteConnection("Data Source=" + path + @"\Data\LyndisLibrary.sqlite;Version=3;"))
        //        {
        //            {
        //                connDB.Open();
        //                SQLiteCommand command = new SQLiteCommand(queryUpdate, connDB);
        //                command.Parameters.Add(new SQLiteParameter("@id", currentFeat.Id));
        //                command.Parameters.Add(new SQLiteParameter("@prerequisite", prereq));
        //                command.ExecuteNonQuery();
        //                connDB.Close();
        //            }
        //        }
        //    }
        //}

        // To be moved later
        // Used to create the prerequisite of the actual feats
        //private void CreatePrerequisiteBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    FeatRepository featRepository = new FeatRepository();
        //    List<Feat> Feats = featRepository.GetFeats();
        //    // prepare the data
        //    // the list of feat, ordered to avoid issues
        //    var linqFeats = Feats.OrderByDescending(x => x.Name.Length);
        //    // 
        //    System.Diagnostics.Debug.WriteLine("[CREATING PREREQUISITE] start");
        //    foreach (Feat feat in Feats)
        //    {
        //        // for each feat in the list, check if there are feat in the prerequisite
        //        if (feat.PrerequisitesBaseString != null)
        //            generateFromFeats(feat, linqFeats);
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
