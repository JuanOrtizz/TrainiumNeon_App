using TrainiumNeon.Views;

namespace TrainiumNeon
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DetalleEjercicio), typeof(DetalleEjercicio));
            Routing.RegisterRoute(nameof(AgregarEditarRutina), typeof(AgregarEditarRutina));
            Routing.RegisterRoute(nameof(AgregarEditarEjercicioRutina), typeof(AgregarEditarEjercicioRutina));
        }
    }
}
