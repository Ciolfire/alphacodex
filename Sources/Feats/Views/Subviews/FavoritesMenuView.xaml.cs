using System.Windows.Controls;

namespace AlphaCodex.Views
{
    /// <summary>
    /// Logique d'interaction pour FavoritesMenuView.xaml
    /// </summary>
    public partial class FavoritesMenuView : UserControl
    {
        public FavoritesMenuView()
        {
            InitializeComponent();
        }

        private void FavoriteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
