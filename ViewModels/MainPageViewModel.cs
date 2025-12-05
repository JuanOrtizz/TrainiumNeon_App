using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TrainiumNeon.Data.Repositories;
using TrainiumNeon.Messages;
using TrainiumNeon.Services;

namespace TrainiumNeon.ViewModels
{
    public class MainPageViewModel: INotifyPropertyChanged, IRecipient<UsuarioMessages.RegistroExistosoMessage>
    {

        // Servicios y repositorio
        private readonly IEstadoContraseniaService _estadoContraseniaService;
        private readonly ISesionService _sesionService;
        private readonly IValidacionService _validacionService;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        // Propiedades privadas
        private string _email = string.Empty;
        private string _contrasenia = string.Empty;
        private string _errorEmail = string.Empty;
        private string _errorContrasenia = string.Empty;
        private bool _hayErrorEnEmail;
        private bool _hayErrorEnContrasenia;
        private bool _contraseniaOculta = true;
        private string _iconoContrasenia = "ver_contrasenia.png";
        private bool _isBusy;

        // Propiedades publicas
        public string Email
        {
            get => _email;
            set
            {
                var nuevoValor = value?.ToLower().Trim() ?? string.Empty;
                if (_email != nuevoValor)
                {
                    _email = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string Contrasenia
        {
            get => _contrasenia;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_contrasenia != nuevoValor)
                {
                    _contrasenia = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorEmail
        {
            get => _errorEmail;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_errorEmail != nuevoValor)
                {
                    _errorEmail = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorContrasenia
        {
            get => _errorContrasenia;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if(_errorContrasenia != nuevoValor)
                {
                    _errorContrasenia = nuevoValor;
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
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_iconoContrasenia != nuevoValor)
                {
                    _iconoContrasenia = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        // Comandos
        public ICommand IniciarSesionCommand { get; }
        public ICommand RegistroCommand { get; }
        public ICommand CambiarEstadoContraseniaCommand { get; }

        public MainPageViewModel(IEstadoContraseniaService estadoContraseniaService, ISesionService sesionService, IValidacionService validacionService, IUsuarioRepositorio usuarioRepositorio)
        {
            // Inicializan servicios y repositorio por DI
            _estadoContraseniaService = estadoContraseniaService;
            _sesionService = sesionService;
            _validacionService = validacionService;
            _usuarioRepositorio = usuarioRepositorio;
            // Inicializan comandos
            IniciarSesionCommand = new Command(async () => await IniciarSesionAsync());
            RegistroCommand = new Command(async () => await RegistrarseAsync());
            CambiarEstadoContraseniaCommand = new Command(MostrarUOcultarContrasenia);
            // Suscripcion a mensajeria de UsuarioMessages
            WeakReferenceMessenger.Default.Register(this);
        }

        // Task asincrona para iniciar sesion
        private async Task IniciarSesionAsync()
        {
            // Muestro spinner de carga
            IsBusy = true;
            // Valido que los campos no esten vacios
            ErrorEmail = _validacionService.ValidarCampoVacio(Email);
            ErrorContrasenia = _validacionService.ValidarCampoVacio(Contrasenia);
            HayErrorEnEmail = !string.IsNullOrEmpty(ErrorEmail);
            HayErrorEnContrasenia = !string.IsNullOrEmpty(ErrorContrasenia);
            
            // Si hay errores oculto el spinner y salgo
            if (HayErrorEnContrasenia || HayErrorEnEmail)
            {
                IsBusy = false;
                return;
            }

            // Obtengo el idUsuario, si es 0 no se pudo iniciar sesion, sino inicia sesion 
            var usuarioId = await _usuarioRepositorio.IniciarSesionAsync(Email, Contrasenia);
            await Task.Delay(500);
            if (usuarioId == 0)
            {
                IsBusy = false; // Oculto el spinner de carga 
                ErrorEmail = "Correo electrónico o contraseña incorrectos.";
                HayErrorEnEmail = true;
                HayErrorEnContrasenia = true;
                return;
            }

            // Guarda el usuario en preferences
            _sesionService.GuardarSesion(usuarioId);
            // Oculto el spinner de carga
            IsBusy = false;

            // Muestro MenuPrincipal al iniciar sesion
            await Shell.Current.GoToAsync("//MenuPrincipal");
        }

        // Task privada para navegar a pagina de registro
        private async Task RegistrarseAsync()
        {
            await Shell.Current.GoToAsync("//Registro");
        }

        //Metodo para mostrar/ocultar la contraseña
        private void MostrarUOcultarContrasenia()
        {
            // Obtengo el estado de la contraseña
            var resultado = _estadoContraseniaService.CambiarEstadoContrasenia(ContraseniaOculta, IconoContrasenia);
            // Actualizo los valores en el viewModel
            ContraseniaOculta = resultado.contraseniaOculta;
            IconoContrasenia = resultado.iconoContrasenia;
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Implementacion de IRecipient para recibir mensaje de registro exitoso
        public async void Receive(UsuarioMessages.RegistroExistosoMessage message)
        {
            await Toast.Make(message.mensaje, ToastDuration.Short).Show();
        }
    }
}
