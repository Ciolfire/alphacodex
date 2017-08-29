using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Data;

namespace AlphaCodex
{

    /// <summary>
    /// Logique d'interaction pour classesPanel.xaml
    /// </summary>
    public partial class ClassesPanel : UserControl
    {
        // properties
        MainWindow root = ((MainWindow)Application.Current.MainWindow);

        public ClassesPanel()
        {
            Feat defaultFeat = new Feat();
            InitializeComponent();
        }

        #region controls
        /* Get the selected class informations and display them */
        private void ClassesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassesListBox.SelectedIndex == -1)
                return;
            else
                ClassBox.Visibility = Visibility.Visible;
            
            // get the current class
            Class currentClass = MainWindow.Classes[ClassesListBox.SelectedIndex];

            // Affect the current class to the panel
            ClassBox.DataContext = currentClass;
            // Hide the picture frame if no picture
            if (currentClass.Picture == null)
            ClassPictureBorder.Visibility = Visibility.Collapsed;

            // game rule informations
            ClassIntroduction.Text = currentClass.Name + " have the following game statistics.";
            ClassAlignmentRestriction.Text = currentClass.AlignmentInfo;
            ClassHitDie.Text = "d" + currentClass.HitDie + ".";
            // Table
            ClassTable.ItemsSource = currentClass.buildTable().DefaultView;
            generateSpells(currentClass.Spells);
            // skills
            generateSkills(currentClass.Skills, currentClass.Name);
            // features
            root.ParseText(currentClass.Features, "class", ClassFeatures);
            // ex
            generateExField(currentClass.Name, currentClass.Ex);
        }

        private void generateSpells(Spellcaster spells)
        {
            if (spells != null)
            {
                if (spells.spellsDaily != null)
                {
                    ClassSpells.Visibility = Visibility.Visible;
                    ClassSpellsDailyTable.ItemsSource = spells.spellsDaily.DefaultView; 
                }
                else
                    ClassSpells.Visibility = Visibility.Collapsed;
                if (spells.spellsKnown != null)
                {
                    ClassSpellsKnown.Visibility = Visibility.Visible;
                    ClassSpellsKnownTable.ItemsSource = spells.spellsKnown.DefaultView;
                }
                else
                    ClassSpellsKnown.Visibility = Visibility.Collapsed;
            }
            else
                ClassSpells.Visibility = Visibility.Collapsed;
        }

        private void generateExField(string name, string ex)
        {
            if (!string.IsNullOrEmpty(ex))
            {
                ClassExFieldTitle.Visibility = Visibility.Visible;
                ClassExFieldTitle.Text = "Ex-" + name;
                ClassExField.Visibility = Visibility.Visible;
                ClassExField.Text = ex;
            }
            else
            {
                ClassExFieldTitle.Visibility = Visibility.Collapsed;
                ClassExField.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region methods
        // Create the skills
        private void generateSkills(List<CharSkill> skills, string name)
        {
            string newSkill;
            ClassSkills.Inlines.Clear();
            foreach (CharSkill skill in skills)
            {
                if (ClassSkills.Inlines.Count == 0)
                    ClassSkills.Inlines.Add("The " + name + "’s class skills (and the key ability for each skill) are ");
                else
                    ClassSkills.Inlines.Add(", ");
                newSkill = skill.name + " (" + skill.keyAbility.shortName + ')';
                ClassSkills.Inlines.Add(newSkill);
            }
            ClassSkills.Inlines.Add(".");
        }


        #endregion
    }
}
