using AlphaCodex.Models;
using AlphaCodex.Tools;
using AlphaCodex.ViewModels;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AlphaCodex.Views
{
    /// <summary>
    /// Logique d'interaction pour FeatsView.xaml
    /// </summary>
    public partial class FeatView : UserControl
    {
        public FeatView()
        {
            this.Visibility = System.Windows.Visibility.Hidden;
            InitializeComponent();
        }

        private void FormatBindingData(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            Grid featView = sender as Grid;
            if (featView.DataContext is FeatViewModel feat)
            {
                this.Visibility = System.Windows.Visibility.Visible;
                JadeTools.ParseText(feat.Benefit, "feat", Benefit);
                JadeTools.ParseText(feat.Normal, "feat", Normal);
                JadeTools.ParseText(feat.Special, "feat", Special);
                JadeTools.ParseText(feat.Note, "feat", Note);
                GeneratePrerequisites(feat);
                GenerateChildren(feat);
                GenerateHistory(feat);
            }
        }

        private void GenerateHistory(FeatViewModel feat)
        {
            History.Inlines.Clear();
            CurrentFeat.Inlines.Clear();
            if (feat.History.Any())
            {
                foreach (LinkViewModel linkVM in feat.History)
                {
                    if (History.Inlines.Count != 0)
                    {
                        History.Inlines.Add(", ");
                    }
                    Hyperlink link = new Hyperlink();
                    link.Inlines.Add(linkVM.Name);
                    link.Command = linkVM.NavigateCommand;
                    History.Inlines.Add(link);
                }
                History.Inlines.Add(".");
            }
            if (feat.CurrentFeat != null)
            {
                Hyperlink link = new Hyperlink();
                link.Inlines.Add(feat.CurrentFeat.Name);
                link.Command = feat.CurrentFeat.NavigateCommand;
                CurrentFeat.Inlines.Add(link);
            }
        }

        private void GenerateChildren(FeatViewModel feat)
        {
            Children.Inlines.Clear();
            if (!feat.Children.Any())
                return;
            foreach (LinkViewModel linkVM in feat.Children)
            {
                if (Children.Inlines.Count != 0)
                {
                    Children.Inlines.Add(", ");
                }
                Hyperlink link = new Hyperlink();
                link.Inlines.Add(linkVM.DisplayName);
                link.Command = linkVM.NavigateCommand;
                Children.Inlines.Add(link);
            }
            Children.Inlines.Add(".");
        }

        private void GeneratePrerequisites(FeatViewModel feat)
        {
            if (feat.PrerequisitesBaseString == null)
                return;
            Prerequisite.Inlines.Clear();

            string[] SplitPrerequisites = Regex.Split(feat.PrerequisitesBaseString, @"({[0-9]+})");
            int id;
            foreach (string prerequisitePart in SplitPrerequisites)
            {
                // Check if this part is dynamic
                if (Regex.Match(prerequisitePart, @"({[0-9]+})") != Match.Empty)
                {
                    // Keep only the id
                    id = int.Parse(Regex.Replace(prerequisitePart, @"[{}]+", ""));
                    // get the associated Link object and use it to generate the Hyperlink
                    Hyperlink link = new Hyperlink();
                    LinkViewModel linkVM = feat.Prerequisites.Single(item => item.LinkId == id);
                    link.Inlines.Add(linkVM.DisplayName);
                    link.Command = linkVM.NavigateCommand;
                    Prerequisite.Inlines.Add(link);
                }
                else
                {
                    Prerequisite.Inlines.Add(prerequisitePart);
                }
            }
        }
    }
}
