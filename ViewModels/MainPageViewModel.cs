using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrainiumNeon.Services;
using TrainiumNeon.Views;

namespace TrainiumNeon.ViewModels
{
    public class MainPageViewModel: INotifyPropertyChanged
    {

        // Servicios
        private readonly IValidacionService _validacionService;
        private readonly IEstadoContraseniaService _estadoContraseniaService;
        // Propiedades privadas
        private INavigation _navigation;
        private string _email;
        private string _contrasenia;
        private string _errorEmail;
        private string _errorContrasenia;
        private bool _hayErrorEnEmail;
        private bool _hayErrorEnContrasenia;
        private bool _contraseniaOculta;
        private string _iconoContrasenia;
        // Propiedades publicas
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Contrasenia
        {
            get => _contrasenia;
            set
            {
                if (_contrasenia != value)
                {
                    _contrasenia = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorEmail
        {
            get => _errorEmail;
            set
            {
                if (_errorEmail != value)
                {
                    _errorEmail = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorContrasenia
        {
            get => _errorContrasenia;
            set
            {
                if (_errorContrasenia != value)
                {
                    _errorContrasenia = value;
                    OnPropertyChanged();
                } 
            }
        }
        public bool HayErrorEnEmail
        {
            get => _hayErrorEnEmail;
            set
            {
                if (_hayErrorEnEmail != value)
                {
                    _hayErrorEnEmail = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool HayErrorEnContrasenia
        {
            get => _hayErrorEnContrasenia;
            set
            {
                if (_hayErrorEnContrasenia != value)
                {
                    _hayErrorEnContrasenia = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool ContraseniaOculta
        {
            get => _contraseniaOculta;
            set
            {
                if (_contraseniaOculta != value)
                {
                    _contraseniaOculta = value;
                    OnPropertyChanged();
                }
            }
        }
        public string IconoContrasenia
        {
            get => _iconoContrasenia;
            set
            {
                if (_iconoContrasenia != value)
                {
                    _iconoContrasenia = value;
                    OnPropertyChanged();
                }
            }
        }

        // Comandos
        public Command RegistroCommand { get; }
        public Command IniciarSesionCommand { get; }
        public Command CambiarEstadoContraseniaCommand { get; }

        public MainPageViewModel(IValidacionService validacionService, IEstadoContraseniaService estadoContraseniaService)
        {
            // Inicializan servicios por DI
            _validacionService = validacionService;
            _estadoContraseniaService = estadoContraseniaService;
            // Inicializa el icono por default
            ContraseniaOculta = true;
            IconoContrasenia = "ver_contrasenia.png";
            // Inicializan comandos
            RegistroCommand = new Command(async () => await Registrarse());
            IniciarSesionCommand = new Command(async() => await IniciarSesion());
            CambiarEstadoContraseniaCommand = new Command(MostrarUOcultarContrasenia);
            // Suscripcion a mensaje de registro exitoso para mostrar toast
            MessagingCenter.Subscribe<RegistroViewModel>(this, "RegistroExitoso", async (sender) =>
            {
                var toast = Toast.Make("Registro exitoso. Ahora inicia sesión.", ToastDuration.Short);
                await toast.Show();
            });
        }

        // Task privada para iniciar sesion
        private async Task IniciarSesion()
        {
            ErrorEmail = _validacionService.ValidarEmail(Email);
            ErrorContrasenia = _validacionService.ValidarContrasenia(Contrasenia);
            HayErrorEnContrasenia = !string.IsNullOrEmpty(ErrorContrasenia);
            HayErrorEnEmail = !string.IsNullOrEmpty(ErrorEmail);

            if (HayErrorEnEmail || HayErrorEnContrasenia)
            {
                return;
            }

            Application.Current.MainPage = new AppShell();
        }

        // Task privada para navegar a pagina de registro
        private async Task Registrarse()
        {
            // Obtiene pagina de registro desde el contenedor de servicios
            var registroPage = App.Current.Handler.MauiContext.Services.GetService<Registro>();

            registroPage.Opacity = 0;
            registroPage.TranslationX = 300;

            await _navigation.PushAsync(registroPage, false);

            await Task.WhenAll(
                registroPage.TranslateTo(0, 0, 300, Easing.SinOut),
                registroPage.FadeTo(1, 500, Easing.CubicInOut)
            );
        }

        //Metodo para mostrar/ocultar la contraseña
        private void MostrarUOcultarContrasenia()
        {
            var resultado = _estadoContraseniaService.CambiarEstadoContrasenia(ContraseniaOculta, IconoContrasenia);
            ContraseniaOculta = resultado.contraseniaOculta;
            IconoContrasenia = resultado.iconoContrasenia;
        }

        //Metodo para inicializar la navegacion desde Code-Behind
        public void SetNavigation(INavigation navigation) => _navigation = navigation;

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
