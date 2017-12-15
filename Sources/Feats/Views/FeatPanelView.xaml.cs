using AlphaCodex.Tools;
using System.Windows;
using System.Windows.Controls;

namespace AlphaCodex.Views
{
    /// <summary>
    /// Logique d'interaction pour FeatPanelView.xaml
    /// </summary>
    public partial class FeatPanelView : UserControl
    {
        public FeatPanelView()
        {
            InitializeComponent();
        }

        private void CopyAsPicture(object sender, RoutedEventArgs e)
        {
            Clipboard.SetImage(JadeTools.CaptureScreen(FeatView.FeatBox, 96.0, 96.0));
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (GroupsView.Visibility == Visibility.Visible) GroupsView.Visibility = Visibility.Collapsed;
            else GroupsView.Visibility = Visibility.Visible;
        }
    }
}
