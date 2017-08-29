using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System;
using System.Windows.Media;

namespace AlphaCodex
{

    /// <summary>
    /// Logique d'interaction pour featsPanel.xaml
    /// </summary>
    public partial class FeatPanel : UserControl
    {
        #region properties
        MainWindow root = ((MainWindow)Application.Current.MainWindow);
        private enum SearchMode
        {
            NAME,
            BENEFIT,
            PREREQUISITE
        }

        private SearchMode currentMode = 0;
        public bool isRightClickActivated = false;

        public FeatPanel()
        {
            InitializeComponent();
        }
        #endregion

        #region methods
        /* check which feats must be displayed */
        private void RefreshFeatList()
        {
            FeatType extraType;
            string searchedField;
            foreach (Feat feat in MainWindow.Feats)
            {
                extraType = GetExtraType(feat);
                searchedField = GetSearchedField(feat);

                if (feat.Current
                    && searchedField.Contains(SearchFeat.Text.ToLower())
                    && SubtypeIsActive(feat)
                    && TypeIsActive(feat, extraType)
                    )
                    feat.Visibility = Visibility.Visible;
                else
                    feat.Visibility = Visibility.Collapsed;
            }
        }

        private bool TypeIsActive(Feat feat, FeatType extraType)
        {
            if (!MainWindow.FeatTypes.Single(type => type.Id == feat.Type).Disabled
                || (extraType != null && !extraType.Disabled)
                || (feat.Fighter && !(MainWindow.FeatTypes.Single(item => item.Name == "Fighter").Disabled)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SubtypeIsActive(Feat feat)
        {
            if (MainWindow.FeatSubtypes.Single(subtype => subtype.Id == feat.SubtypeId).Disabled)
                return false;
            else
                return true;
        }

        private FeatType GetExtraType(Feat feat)
        {
            if (feat.TypeExtra != 0)
            {
                return MainWindow.FeatTypes.Single(type => type.Id == feat.TypeExtra);
            }
            else
            {
                return null;
            }
        }

        private string GetSearchedField(Feat feat)
        {
            switch (currentMode)
            {
                case SearchMode.NAME:
                    return feat.Name.ToLower();
                case SearchMode.BENEFIT:
                        return feat.Benefit.ToLower();
                case SearchMode.PREREQUISITE:
                    return feat.SimplePrerequisite.ToLower();
                default:
                    return feat.Name.ToLower();
            }
        }


        // Create the prerequisites links
        private void GeneratePrerequisites(Feat currentFeat)
        {
            FeatPrerequisite.Inlines.Clear();
            Collection<string> tmpPre = new Collection<string>();

            string[] toSplit = Regex.Split(currentFeat.Prerequisite, @"({[0-9]+})");
            int id;
            foreach (string part in toSplit)
            {
                // Check if it's a variable
                if (Regex.Match(part, @"({[0-9]+})") != Match.Empty)
                {
                    // if it is create a new link
                    Hyperlink link = new Hyperlink();
                    // Keep only the id
                    id = int.Parse(Regex.Replace(part, @"[{}]+", ""));
                    // This will need to be edited if others kind of prerequisite are linked (ie, ability, skills, ...)
                    // get the next prerequisite feat based on the linkId
                    FeatPrerequisite prerequisite = MainWindow.FeatPrerequisites.Single(prereq => (prereq.ChildId == currentFeat.Id && prereq.LinkId == id));
                    // search the parent feat name by it's id
                    link.Inlines.Add(MainWindow.Feats.Single(feat => feat.Id == prerequisite.ParentId).Name);
                    link.TargetName = prerequisite.ParentId.ToString();
                    link.Tag = "feat";
                    link.Click += Link_Click;
                    FeatPrerequisite.Inlines.Add(link);
                }
                else {
                    FeatPrerequisite.Inlines.Add(part);
                }
            }
        }

        // Create the childrens links
        private void GenerateChildrens(Feat currentFeat)
        {
            Feat candidate;
            FeatRequiredFor.Inlines.Clear();
            foreach (int featId in currentFeat.Children)
            {
                Hyperlink link = new Hyperlink();
                candidate = MainWindow.Feats.Single(feat => feat.Id == featId);
                if (candidate.Current == true) {
                    if (FeatRequiredFor.Inlines.Count != 0)
                    {
                        FeatRequiredFor.Inlines.Add(", ");
                    }
                    link.Inlines.Add(candidate.Name);
                    link.TargetName = featId.ToString();
                    link.Tag = "feat";
                    link.Click += Link_Click;
                    FeatRequiredFor.Inlines.Add(link);
                }
            }
            if (currentFeat.Children.Any())
                FeatRequiredFor.Inlines.Add(".");
        }

        // Create the history links
        private void GenerateHistory(Feat currentFeat)
        {
            Feat candidate;
            FeatHistory.Inlines.Clear();
            if (currentFeat.Current == true && currentFeat.Older != null)
            {
                FeatHistoryLabel.Text = "Previously appeared in:";
                foreach (int featId in currentFeat.Older)
                {
                    Hyperlink link = new Hyperlink();
                    candidate = MainWindow.Feats.Single(feat => feat.Id == featId);
                    if (currentFeat.Current == true)
                    {
                        if (FeatHistory.Inlines.Count != 0)
                            FeatHistory.Inlines.Add(", ");
                        link.Inlines.Add(candidate.Book.Name);
                        link.TargetName = featId.ToString();
                        link.Tag = "feat";
                        link.Click += Link_Click;
                        FeatHistory.Inlines.Add(link);
                    }
                }
                FeatHistory.Inlines.Add(".");
            }
            else if (currentFeat.Current == false) {
                FeatHistoryLabel.Text = "Revised in:";
                Hyperlink link = new Hyperlink();
                candidate = MainWindow.Feats.Single(feat => feat.Id == currentFeat.CurrentId);
                if (candidate.Current == true)
                {
                    link.Inlines.Add(candidate.Book.Name);
                    link.TargetName = candidate.Id.ToString();
                    link.Tag = "feat";
                    link.Click += Link_Click;
                    FeatHistory.Inlines.Add(link);
                    FeatHistory.Inlines.Add(".");
                }
            }
        }

        #endregion

        #region controls
        /* Get the selected feat informations and display them */
        private void FeatsBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FeatsBox.SelectedIndex == -1)
                return;
            else
                FeatBox.Visibility = Visibility.Visible;
            Feat currentFeat = MainWindow.Feats[FeatsBox.SelectedIndex];
            string title = currentFeat.Name + " [" + MainWindow.FeatTypes.Single(item => item.Id == currentFeat.Type).Name;

            FeatBook.Text = currentFeat.Book.Name + " page " + currentFeat.Page;

            FeatBox.DataContext = currentFeat;
            if (currentFeat.TypeExtra != 0)
                title += ", " + MainWindow.FeatTypes.Single(item => item.Id == currentFeat.TypeExtra).Name + "]";
            else
                title += "]";
            featTitle.Text = title;
            root.ParseText(currentFeat.Description, "feat", FeatDescription);
            root.ParseText(currentFeat.Benefit, "feat", FeatBenefit);
            root.ParseText(currentFeat.Normal, "feat", FeatNormal);
            root.ParseText(currentFeat.Special, "feat", FeatSpecial);
            root.ParseText(currentFeat.Note, "feat", FeatNote);

            // fetch the prerequisite string and split it 
            GeneratePrerequisites(currentFeat);
            GenerateChildrens(currentFeat);
            GenerateHistory(currentFeat);
            FeatsBox.SelectedIndex = -1;
            SetGlobalVisibility(currentFeat);
        }

        // Set the visibility for every element of the FeatBox
        private void SetGlobalVisibility(Feat currentFeat)
        {
            if (currentFeat.Current)
                featStatut.Visibility = Visibility.Collapsed;
            else
                featStatut.Visibility = Visibility.Visible;

            if (currentFeat.Prerequisite.Any())
                FeatPrerequisiteHeader.Visibility = Visibility.Visible;
            else
                FeatPrerequisiteHeader.Visibility = Visibility.Collapsed;
            if (currentFeat.Normal.Any())
                FeatNormalHeader.Visibility = Visibility.Visible;
            else
                FeatNormalHeader.Visibility = Visibility.Collapsed;
            if (currentFeat.Special.Any())
                FeatSpecialHeader.Visibility = Visibility.Visible;
            else
                FeatSpecialHeader.Visibility = Visibility.Collapsed;
            if (currentFeat.Note.Any())
                FeatNoteHeader.Visibility = Visibility.Visible;
            else
                FeatNoteHeader.Visibility = Visibility.Collapsed;
            if (currentFeat.Children.Count == 0)
                FeatRequiredForHeader.Visibility = Visibility.Collapsed;
            else
                FeatRequiredForHeader.Visibility = Visibility.Visible;
            if (currentFeat.Older == null && currentFeat.Current == true)
                FeatHistoryHeader.Visibility = Visibility.Collapsed;
            else
                FeatHistoryHeader.Visibility = Visibility.Visible;
        }

        /* Recherche textuelle */
        private void SearchFeat_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            RefreshFeatList();
        }

        /* Recherche par type */
        private void Type_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image image = (Image)sender;

            if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                FeatType tmp = MainWindow.FeatTypes.Single(item => item.Id.ToString() == image.Tag.ToString());

                if (image.Opacity.Equals(1))
                    tmp.Disabled = false;
                else
                    tmp.Disabled = true;
                isRightClickActivated = false;
            }
            else
            {
                bool state = false;
                bool changed = false;

                if (image.Opacity.Equals(1) || isRightClickActivated == false)
                {
                    state = true;
                    isRightClickActivated = true;
                }
                else if (isRightClickActivated == true)
                {
                    isRightClickActivated = false;
                    changed = true;
                }
                foreach (FeatType type in MainWindow.FeatTypes)
                {
                    if (type.Id.ToString() == image.Tag.ToString())
                    {
                        if (state.Equals(true))
                            type.Disabled = false;
                        else if (changed != true)
                            type.Disabled = true;
                        else
                            type.Disabled = false;
                    }
                    else
                        type.Disabled = state;
                }
            }
            RefreshFeatList();
        }

        /* Recherche par sous-type */
        private void Subtype_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            String searchedId = image.Tag.ToString();

            if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                FeatSubtype tmp = MainWindow.FeatSubtypes.Single(item => item.Id.ToString() == image.Tag.ToString());

                if (image.Opacity.Equals(1))
                    tmp.Disabled = false;
                else
                    tmp.Disabled = true;
                isRightClickActivated = false;
            }
            else
            {
                bool state = false;
                bool changed = false;

                if (image.Opacity.Equals(1) || isRightClickActivated == false)
                {
                    state = true;
                    isRightClickActivated = true;
                }
                else if (isRightClickActivated == true)
                {
                    isRightClickActivated = false;
                    changed = true;
                }

                foreach (FeatSubtype subtype in MainWindow.FeatSubtypes)
                {
                    if (subtype.Id.ToString() == searchedId)
                    {
                        if (state == true)
                            subtype.Disabled = false;
                        else if (changed != true)
                            subtype.Disabled = true;
                        else
                            subtype.Disabled = false;
                    }
                    else
                        subtype.Disabled = state;
                }
            }
            RefreshFeatList();
        }

        /* Affiche les informations détaillées du don */
        private void FeatOptions_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            FeatOptionsBlock.Visibility = Visibility.Visible;
        }

        /* Masque les informations détaillées*/
        private void FeatOptions_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            FeatOptionsBlock.Visibility = Visibility.Collapsed;
        }

        /* happens when a feat link is clicked */
        private void Link_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)sender;
            int id = int.Parse(link.TargetName);
            string type = link.Tag.ToString();
            if (type == "feat")
            {
                FeatsBox.SelectedItem = MainWindow.Feats.Where(feat => feat.Id == id).First();
            }
        }

        private void TypeHeader_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TypesBox.Visibility == Visibility.Visible)
                TypesBox.Visibility = Visibility.Collapsed;
            else
                TypesBox.Visibility = Visibility.Visible;
        }

        private void SubtypesHeader_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SubtypesBox.Visibility == Visibility.Visible)
            {
                SubtypesBox.Visibility = Visibility.Collapsed;
            }
            else
                SubtypesBox.Visibility = Visibility.Visible;
        }

        private void StackPanelCopyT_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string prerequisite = string.Empty;
            string normal = string.Empty;
            string special = string.Empty;
            string note = string.Empty;

            if (FeatPrerequisite.Text != string.Empty)
                prerequisite = "Prerequisite: " + FeatPrerequisite.Text + "\n\n";
            if (FeatNormal.Text != string.Empty)
                normal = "Normal: " + FeatNormal.Text + "\n\n";
            if (FeatSpecial.Text != string.Empty)
                special = "Special: " + FeatSpecial.Text + "\n\n";
            if (FeatNote.Text != string.Empty)
                note = "Note: " + FeatNote.Text + "\n\n";


            string featText = featTitle.Text + "\n\n";
            featText += FeatDescription.Text + "\n\n";
            featText += prerequisite;
            featText += "Benefit: " + FeatBenefit.Text + "\n\n";
            featText += normal + special + note;
            featText += FeatBook.Text;
            Clipboard.SetText(featText);
        }

        private void StackPanelCopyP_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FeatOptionsGrid.Visibility = Visibility.Collapsed;
            FeatBook.Visibility = Visibility.Visible;
            Clipboard.SetImage(MainWindow.CaptureScreen(FeatBox, 96.0, 96.0));
            FeatOptionsGrid.Visibility = Visibility.Visible;
            FeatBook.Visibility = Visibility.Hidden;
        }

        private void SearchMode_Checked(object sender, RoutedEventArgs e)
        {
            if ((Boolean)SearchNameRB.IsChecked)
                currentMode = SearchMode.NAME;
            else if ((Boolean)SearchPrerequisiteRB.IsChecked)
                currentMode = SearchMode.PREREQUISITE;
            else if ((Boolean)SearchBenefitRB.IsChecked)
                currentMode = SearchMode.BENEFIT;
            if (MainWindow.Feats != null)
                RefreshFeatList();
        }

        private void LightCategory_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
        #endregion
    }
}
