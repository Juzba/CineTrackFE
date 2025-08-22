using System.Windows;
using System.Windows.Controls;

namespace CineTrackFE.Views
{
    /// <summary>
    /// Interakční logika pro FilmDetailView.xaml
    /// </summary>
    public partial class FilmDetailView : UserControl
    {
        public FilmDetailView()
        {
            InitializeComponent();
        }

        private void ShowPopupButton_Click(object sender, RoutedEventArgs e)
        {
            if (!MyPopup.IsOpen)
            {
                MyPopup.IsOpen = true;
            }


        }
    }
}
