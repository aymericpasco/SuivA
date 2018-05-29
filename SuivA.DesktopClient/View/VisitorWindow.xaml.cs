using System.Windows;
using SuivA.DesktopClient.View.UserControl;

namespace SuivA.DesktopClient.View
{
    /// <summary>
    ///     Logique d'interaction pour VisitorWindow.xaml
    /// </summary>
    public partial class VisitorWindow : Window
    {
        public VisitorWindow()
        {
            InitializeComponent();
        }

        private void VisitView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            DataContext = new Visit();
        }
    }
}