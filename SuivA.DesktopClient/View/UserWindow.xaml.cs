using System.Windows;
using SuivA.DesktopClient.View.UserControl;

namespace SuivA.DesktopClient.View
{
    /// <summary>
    ///     Logique d'interaction pour UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
        }

        private void OfficeView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            DataContext = new Office();
        }

        private void DoctorView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            DataContext = new Doctor();
        }
    }
}