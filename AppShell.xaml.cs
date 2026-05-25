using TrainiumNeon.Views;

namespace TrainiumNeon
{
    public partial class AppShell : Shell
    {
        // Variable para controlar si la navegación esta permitida (si no hay internet y no hubo sincronizacion no la tiene que permitir)
        private bool _navegacionPermitida = true;

        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DetalleEjercicio), typeof(DetalleEjercicio));
            Routing.RegisterRoute(nameof(AgregarEditarRutina), typeof(AgregarEditarRutina));
            Routing.RegisterRoute(nameof(AgregarEditarEjercicioRutina), typeof(AgregarEditarEjercicioRutina));
        }

        // Metodo para permitir o bloquear la navegacion en el Shell
        public void PermitirNavegacionEnShell(bool permitido)
        {
            _navegacionPermitida = permitido;
        }

        // Sobrescribo el metodo OnNavigating para controlar la navegacion
        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            // Si la navegacion no esta permitida, cancelo la navegacion
            if (!_navegacionPermitida)
            {
                args.Cancel();
                return;
            }

            base.OnNavigating(args);
        }
    }
}
