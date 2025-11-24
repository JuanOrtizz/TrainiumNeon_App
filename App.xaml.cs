using TrainiumNeon.Services;
using TrainiumNeon.Views;

namespace TrainiumNeon
{
    public partial class App : Application
    {
        // Campos de solo lectura para DI
        private readonly MainPage _mainPage;
        private readonly ISesionService _sesionService;

        public App(MainPage mainPage, ISesionService sesionService)
        {
            InitializeComponent();
            // Inicializan los campos con DI
            _mainPage = mainPage;
            _sesionService = sesionService;
        }

        // Sobrescribo el metodo CreateWindow para inicializar Shell
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        // Sobrescribo le metodo OnStart para decidir en que view arranca dependiendo si esta logeado o no
        protected override void OnStart()
        {
            var id = _sesionService.ObtenerSesion();

            if (id != 0)
            {
                Shell.Current?.GoToAsync("//MenuPrincipal");
            }
            else
            {
                Shell.Current?.GoToAsync("//Login");
            }
        }

    }
}