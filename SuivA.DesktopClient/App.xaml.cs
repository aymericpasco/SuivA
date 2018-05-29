using System.Windows;
using SuivA.DesktopClient.View;

namespace SuivA.DesktopClient
{
    /// <summary>
    ///     Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // var bs = new Bootstrapper();
            // bs.Run();

            // var container = new UnityContainer();

            // container.RegisterType<IVisitContext, VisitContext>();
            // container.RegisterType<VisitViewModel>();

            // container.RegisterType<IDoctorContext, DoctorContext>();
            // container.RegisterType<DoctorViewModel>();

            // container.RegisterType<IOfficeContext, OfficeContext>();
            // container.RegisterType<OfficeViewModel>();

            // var window = new MainWindow();

            var window = new LoginWindow();

            // var window = new DoctorWindow
            //var window = new OfficeWindow

            // DataContext = container.Resolve<VisitViewModel>()
            // DataContext = container.Resolve<DoctorViewModel>()
            // DataContext = container.Resolve<OfficeViewModel>()

            window.ShowDialog();
        }
    }
}