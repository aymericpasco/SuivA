using SuivA.Data.Context;
using SuivA.Data.Context.Interface;
using SuivA.DesktopClient.ViewModel;
using Unity;

namespace SuivA.DesktopClient.View.UserControl
{
    /// <summary>
    ///     Logique d'interaction pour Office.xaml
    /// </summary>
    public partial class Office : System.Windows.Controls.UserControl
    {
        public Office()
        {
            InitializeComponent();

            var container = new UnityContainer();
            container.RegisterType<IOfficeContext, OfficeContext>();
            container.RegisterType<OfficeViewModel>();

            DataContext = container.Resolve<OfficeViewModel>();
        }
    }
}