using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlphaCodex
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Class DefaultClass { get; set; }
        public static Feat DefaultFeat {get; set;}
        public static Player Player { get; set; }
        public static List<Alignment> Alignments { get; set; }
        public static List<Skill> Skills { get; set; }
        public static List<Ability> Abilities { get; set; }

        public MainWindow()
        {
            DataContext = this;
            DefaultClass = new Class();
            DefaultFeat = new Feat();
            InitializeData();
            InitializeComponent();
            if (Player.Name == string.Empty)
            {
                menu.SelectedIndex = 0;
                Welcome.Visibility = Visibility.Visible;
            } else
            {
                menu.IsEnabled = true;
            }
            /*tmp */
            RacesPanel.Visibility = Visibility.Collapsed;
            convPanel.Visibility = Visibility.Collapsed;
            //homePanel.Visibility = Visibility.Collapsed;
            //classesPanel.Visibility = Visibility.Collapsed;
            //ajouts.Visibility = Visibility.Collapsed;

            /**/
            //refreshClassList();
        }

        private int InitializeData()
        {
            GetPlayer();
            GetAlignments();
            GetAbilities();
            GetSkills();
            GetBooks();
            //getSpellSchoolList();
            //getSpellDescriptorList();
            //getSpellcasterList();
            //getSpellDomainList();
            return (0);
        }

        // Get all the childrens of a control
        public List<Control> AllChildren(DependencyObject parent)
        {
            var _List = new List<Control> { };
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is Control)
                    _List.Add(_Child as Control);
                _List.AddRange(AllChildren(_Child));
            }
            return _List;
        }

        #region temp
 
            //// Code to keep, extract some controls from a parents from their name, very useful with template (for example listBoxItem) //
            //var _ListBox = spellcasterListBoxAdd as ListBox;
            //foreach (var _ListBoxItem in _ListBox.SelectedItems)
            //{
            //    var _Container = _ListBox.ItemContainerGenerator.ContainerFromItem(_ListBoxItem);
            //    var _Children = AllChildren(_Container);
            //    var _Name = "level";
            //    var _Control = (TextBox)_Children.First(c => c.Name == _Name);
            //    //spellByClass.Add(new KeyValuePair<int, string>(_class.id, _Control.Text));
            //    //spellLevel += _class.id.ToString() + "-" + _Control.Text + ",";
            //    _Control.Text = null;
            //}
            //spellcasterListBoxAdd.SelectedIndex = -1;
            //// Code to keep, extract some controls from a parents from their name, very useful with template (for example listBoxItem) //

            //_ListBox = domainsListBoxAdd as ListBox;
            //foreach (var _ListBoxItem in _ListBox.SelectedItems)
            //{
            //    var _Container = _ListBox.ItemContainerGenerator.ContainerFromItem(_ListBoxItem);
            //    var _Children = AllChildren(_Container);
            //    var _Name = "level";
            //    var _Control = (TextBox)_Children.First(c => c.Name == _Name);
            //    var _class = (domain)_ListBoxItem;
            //    spellLevel += "d" + _class.id.ToString() + "-" + _Control.Text + ",";
            //    _Control.Text = null;
            //}

        // generate the feats requirements from feats
        private void generateFromFeats(Feat currentFeat, IOrderedEnumerable<Feat> linq)
        {
            int linkId;
            // check if the feat already has saved parents, if yes, get the last index
            if (FeatPrerequisites.Any(x => x.ChildId == currentFeat.Id))
                linkId = FeatPrerequisites.Last(x => x.ChildId == currentFeat.Id).LinkId + 1;
            else
                linkId = 0;
            string prereq = currentFeat.Prerequisite;
            // Note: A child is a feat which HAS prerequisite, a parent is a feat NEEDED by another feat
            string queryInsert = "INSERT INTO feat_prerequisite (child_id, type, link_id, parent_id) VALUES (@child, @type, @link, @parent)";
            string queryUpdate = "UPDATE feat SET 'prerequisite' = @prerequisite WHERE feat.id = @id";

            foreach (Feat feat in linq)
            {
                // if the feat is in the list of prerequisite, it's a parent
                if (feat.Current)
                {
                    if (prereq.Contains(feat.Name) && currentFeat.Id != feat.Id)
                    {
                        System.Diagnostics.Debug.WriteLine("[child] " + currentFeat.Name + " -> [parent] " + feat.Name);
                        prereq = prereq.Replace(feat.Name, "{" + linkId + "}");
                        // add the feat to the list of prerequisite with id_child = currentFeat.Id, id_parent = feat.Id, and type = 'feat'
                        using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
                        {
                            connDB.Open();
                            SQLiteCommand command = new SQLiteCommand(queryInsert, connDB);
                            command.Parameters.Add(new SQLiteParameter("@child", currentFeat.Id));
                            command.Parameters.Add(new SQLiteParameter("@type", "feat"));
                            command.Parameters.Add(new SQLiteParameter("@link", linkId));
                            command.Parameters.Add(new SQLiteParameter("@parent", feat.Id));

                            command.ExecuteNonQuery();
                            connDB.Close();
                        }
                        //MessageBox.Show("before: |" + old + "| after: |" + input + "|");
                        linkId++;
                    }
                }
            }
            if (prereq != currentFeat.Prerequisite)
            {
                using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
                {
                    connDB.Open();
                    SQLiteCommand command = new SQLiteCommand(queryUpdate, connDB);
                    command.Parameters.Add(new SQLiteParameter("@id", currentFeat.Id));
                    command.Parameters.Add(new SQLiteParameter("@prerequisite", prereq));
                    command.ExecuteNonQuery();
                    connDB.Close();
                }
            }
        }

        #endregion

        // To be moved later
        // Used to create the prerequisite of the actual feats
        private void CreatePrerequisiteBtn_Click(object sender, RoutedEventArgs e)
        {
            // prepare the data
            // the list of feat, ordered to avoid issues
            var linqFeats = Feats.OrderByDescending(x => x.Name.Length);
            // 
            System.Diagnostics.Debug.WriteLine("[CREATING PREREQUISITE] start");
            foreach (Feat feat in Feats)
            {
                // for each feat in the least, check if there are feat in the prerequisite
                generateFromFeats(feat, linqFeats);
                // the idea is to check every type of prerequisite, feats, race, skill, and so on
                // Only feat are implemented for now
            }
            System.Diagnostics.Debug.WriteLine("[CREATING PREREQUISITE] Done");

            // [IMPROVE] Should reload from database, to avoid duplicate entries etc.
            // Or maybe not, this is a dev feature only, maybe force quit application once completed.
            // [BUG] Still feat are observable but don't update as intended, to be corrected.
        }

        private void AddSpellPanel_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterSpell();
        }

        // To be moved later
        private void WelcomeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (WelcomeTextBox.Text != string.Empty)
            {
                WelcomeButton.IsEnabled = true;
            } else
            {
                WelcomeButton.IsEnabled = false;
            }
        }

        // To be moved later
        private void WelcomeButton_Click(object sender, RoutedEventArgs e)
        {
            Player.SetName(WelcomeTextBox.Text);
            menu.IsEnabled = true;
            Welcome.Visibility = Visibility.Collapsed;
        }
    }
}
