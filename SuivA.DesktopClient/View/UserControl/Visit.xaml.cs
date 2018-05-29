using SuivA.Data.Context;
using SuivA.Data.Context.Interface;
using SuivA.DesktopClient.ViewModel;
using Unity;

namespace SuivA.DesktopClient.View.UserControl
{
    /// <summary>
    ///     Logique d'interaction pour Visit.xaml
    /// </summary>
    public partial class Visit : System.Windows.Controls.UserControl
    {
        public Visit()
        {
            InitializeComponent();

            var container = new UnityContainer();
            container.RegisterType<IVisitContext, VisitContext>();
            container.RegisterType<VisitViewModel>();

            DataContext = container.Resolve<VisitViewModel>();
        }
    }
}