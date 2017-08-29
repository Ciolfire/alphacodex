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

        private ObservableCollection<Spell> _spellList = new ObservableCollection<Spell>();
        public ObservableCollection<Spell> spellList
        {
            get { return _spellList; }
            set
            {
                _spellList = value;
                NotifyPropertyChanged("spellList");
            }
        }


        #region feat initialization
        private void spellPanel_Loaded(object sender, System.EventArgs e)
        {
           // getSpellList();
        }
        #endregion

        #region List

        /* Retrieve the list of spells from the DB*/
        private void getSpellList()
        {
            string queryString = "SELECT * FROM 'spell' ORDER BY name";

            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();
                spellList.Clear();
                while (reader.Read())
                {
                    spellList.Add(new Spell(reader, SpellSchoolList, spellDescriptorList));
                }
                reader.Close();
                connDB.Close();
            }
        }

        /* Get the selected spell informations and display them */
        private void spellListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //if (spellListBox.SelectedIndex == -1)
            //    return;
            //else
            //    spellBox.Visibility = Visibility.Visible;
            //Spell tmp = spellList[spellListBox.SelectedIndex];

            //spellBox.DataContext = tmp;
            //spellTitle.Text = tmp.name;
            //spellSchool.Text = tmp.school.name;
            //if (tmp.subschool != null)
            //    spellSchool.Text += "(" + tmp.subschool.name + ")";
            ////spellDescriptorListBox.DataContext = tmp.descriptor;
            //spellDescriptorListBox.ItemsSource = tmp.descriptor;
            //spellListBox.SelectedIndex = -1;
        }
        #endregion

        #region methods
        /* check which spells must be displayed */
        private void refreshSpellList()
        {
            //string curName;

            //foreach (Spell spell in spellList)
            //{
            //    curName = spell.name.ToLower();

            //    if (curName.Contains(searchSpell.Text.ToLower()) )
            //        //&& featSubtypesList.First(item => item.id == curID).disabled == 0
            //        //&& featTypesList.First(item => item.id == typeID).disabled == 0)
            //    {
            //        spell.visibility = Visibility.Visible;
            //    }
            //    else if (spell.visibility != Visibility.Collapsed)
            //    {
            //        spell.visibility = Visibility.Collapsed;
            //    }
            //}
        }
        #endregion

        #region controls
        /* Recherche textuelle */
        private void searchSpell_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            refreshSpellList();
        }

        /* Affiche les informations détaillées du sort */
        private void spellOptions_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //spellOptionsBlock.Visibility = Visibility.Visible;
        }

        /* Masque les informations détaillées*/
        private void spellOptions_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //spellOptionsBlock.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}