using System.Windows;
using SuivA.Data.Utility.Session;
using SuivA.DesktopClient.Service;

namespace SuivA.DesktopClient.View
{
    /// <summary>
    ///     Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ConnexionButton_Click(object sender, RoutedEventArgs e)
        {
            // Data.Entity.DataContext.SetDatabaseHost(DbHost.Text); // DEBUG

            /*if (BypassCheck.IsChecked.GetValueOrDefault()) // BYPASS LDAP+MYSQL CONNECTION - DEBUG ONLY
            {
                UserSession.Id = 1;
                UserSession.Username = "apasco";
                UserSession.Visitor = false;
            }
            else // REAL CASE
            {
                var verifiedWithActiveDirectory =
                    LoginService.Authenticated(LoginTextbox.Text, PasswordTextBox.Password, DomainTextbox.Text);

                if (verifiedWithActiveDirectory)
                    LoginService.SaveUserSession(LoginTextbox.Text, PasswordTextBox.Password, DomainTextbox.Text);
            }*/

            var verifiedWithActiveDirectory =
                LoginService.Authenticated(LoginTextbox.Text, PasswordTextBox.Password, DomainTextbox.Text);

            if (verifiedWithActiveDirectory)
                LoginService.SaveUserSession(LoginTextbox.Text, PasswordTextBox.Password, DomainTextbox.Text);

            if (UserSession.Username != null)
            {
                Window v = new UserWindow();
                if (UserSession.Visitor) v = new VisitorWindow();
                Close();
                v.ShowDialog();
            }

            NotificationTextblock.Text =
                "Un problème est survenu lors de la tentative de connexion. Vérifiez les champs et reéssayez.";
        }
    }
}