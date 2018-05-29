using SuivA.Data.Context;
using SuivA.Data.Context.Interface;
using SuivA.DesktopClient.ViewModel;
using Unity;

namespace SuivA.DesktopClient.View.UserControl
{
    /// <summary>
    ///     Logique d'interaction pour Doctor.xaml
    /// </summary>
    public partial class Doctor : System.Windows.Controls.UserControl
    {
        public Doctor()
        {
            InitializeComponent();

            var container = new UnityContainer();
            container.RegisterType<IDoctorContext, DoctorContext>();
            container.RegisterType<DoctorViewModel>();

            DataContext = container.Resolve<DoctorViewModel>();
        }
    }
}