using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TrainiumNeon.Data.Repositories;
using TrainiumNeon.Models;
using TrainiumNeon.Services;

namespace TrainiumNeon.ViewModels
{
    public class PerfilViewModel : INotifyPropertyChanged
    {
        // Servicios y repositorios 
        private readonly IValidacionService _validacionService;
        private readonly ISesionService _sesionService;
        private readonly IEstadoContraseniaService _estadoContraseniaService;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        // Propiedades privadas
        private int _idUsuarioActivo;
        private UsuarioModel _usuario;
        private string _nombre = string.Empty;
        private string _email = string.Empty;
        private string _nuevaContrasenia = string.Empty;
        private string _confirmarNuevaContrasenia = string.Empty;
        private string _errorNuevoNombre = string.Empty;
        private string _errorNuevoEmail = string.Empty;
        private string _errorNuevaContrasenia = string.Empty;
        private string _errorConfirmarNuevaContrasenia = string.Empty;
        private bool _hayErrorEnNombre;
        private bool _hayErrorEnEmail;
        private bool _hayErrorEnNuevaContrasenia;
        private bool _hayErrorEnConfirmarNuevaContrasenia;
        private bool _nuevaContraseniaOculta;
        private bool _confirmarNuevaContraseniaOculta;
        private string _iconoNuevaContrasenia = "ver_contrasenia.png";
        private string _iconoConfirmarNuevaContrasenia = "ver_contrasenia.png";
        private bool _isBusy;
        private bool _puedeActualizar = false;

        // Propiedades publicas
        public int IdUsuarioActivo
        {
            get => _idUsuarioActivo;
            set
            {
                if (_idUsuarioActivo != value)
                {
                    _idUsuarioActivo = value;
                    OnPropertyChanged();
                }
            }
        }
        public UsuarioModel Usuario
        {
            get => _usuario;
            set
            {
                if(_usuario != value)
                {
                    _usuario = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Nombre
        {
            get => _nombre;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_nombre != nuevoValor)
                {
                    _nombre = nuevoValor;
                    _ = EstadoEdicionDeDatos();
                    OnPropertyChanged();
                }
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                var nuevoValor = value?.ToLower().Trim() ?? string.Empty;
                if (_email != nuevoValor)
                {
                    _email = nuevoValor;
                    _ = EstadoEdicionDeDatos();
                    OnPropertyChanged();
                }
            }
        }
        public string NuevaContrasenia
        {
            get => _nuevaContrasenia;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_nuevaContrasenia != nuevoValor)
                {
                    _nuevaContrasenia = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ConfirmarNuevaContrasenia
        {
            get => _confirmarNuevaContrasenia;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_confirmarNuevaContrasenia != nuevoValor)
                {
                    _confirmarNuevaContrasenia = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorNuevoNombre
        {
            get => _errorNuevoNombre;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_errorNuevoNombre != nuevoValor)
                {
                    _errorNuevoNombre = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorNuevoEmail
        {
            get => _errorNuevoEmail;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_errorNuevoEmail != nuevoValor)
                {
                    _errorNuevoEmail = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorNuevaContrasenia
        {
            get => _errorNuevaContrasenia;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_errorNuevaContrasenia != nuevoValor)
                {
                    _errorNuevaContrasenia = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorConfirmarNuevaContrasenia
        {
            get => _errorConfirmarNuevaContrasenia;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_errorConfirmarNuevaContrasenia != nuevoValor)
                {
                    _errorConfirmarNuevaContrasenia = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public bool HayErrorEnNuevoNombre
        {
            get => _hayErrorEnNombre;
            set
            {
                if (_hayErrorEnNombre != value)
                {
                    _hayErrorEnNombre = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool HayErrorEnNuevoEmail
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
        public bool HayErrorEnNuevaContrasenia
        {
            get => _hayErrorEnNuevaContrasenia;
            set
            {
                if (_hayErrorEnNuevaContrasenia != value)
                {
                    _hayErrorEnNuevaContrasenia = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool HayErrorEnConfirmarNuevaContrasenia
        {
            get => _hayErrorEnConfirmarNuevaContrasenia;
            set
            {
                if (_hayErrorEnConfirmarNuevaContrasenia != value)
                {
                    _hayErrorEnConfirmarNuevaContrasenia = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool NuevaContraseniaOculta
        {
            get => _nuevaContraseniaOculta;
            set
            {
                if (_nuevaContraseniaOculta != value)
                {
                    _nuevaContraseniaOculta = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool ConfirmarNuevaContraseniaOculta
        {
            get => _confirmarNuevaContraseniaOculta;
            set
            {
                if (_confirmarNuevaContraseniaOculta != value)
                {
                    _confirmarNuevaContraseniaOculta = value;
                    OnPropertyChanged();
                }
            }
        }
        public string IconoNuevaContrasenia
        {
            get => _iconoNuevaContrasenia;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_iconoNuevaContrasenia != nuevoValor)
                {
                    _iconoNuevaContrasenia = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string IconoConfirmarNuevaContrasenia
        {
            get => _iconoConfirmarNuevaContrasenia;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_iconoConfirmarNuevaContrasenia != nuevoValor)
                {
                    _iconoConfirmarNuevaContrasenia = nuevoValor;
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
        public bool PuedeActualizar
        {
            get => _puedeActualizar;
            set
            {
                if(_puedeActualizar != value)
                {
                    _puedeActualizar = value;
                    OnPropertyChanged();
                }
            }
        }

        //Comandos
        public ICommand CambiarDatosPersonalesCommand { get; }
        public ICommand CambiarContraseniaCommand { get; }
        public ICommand CambiarEstadoNuevaContraseniaCommand { get; }
        public ICommand CambiarEstadoConfirmarNuevaContraseniaCommand { get; }
        public ICommand CerrarSesionCommand { get; }

        // Constructor
        public PerfilViewModel(IValidacionService validacionService, IEstadoContraseniaService estadoContraseniaService, IUsuarioRepositorio usuarioRepositorio, ISesionService sesionService)
        {
            // Inicializan servicios y repositorios por DI
            _validacionService = validacionService;
            _estadoContraseniaService = estadoContraseniaService;
            _usuarioRepositorio = usuarioRepositorio;
            _sesionService = sesionService;
            // Inicializan comandos
            CambiarDatosPersonalesCommand = new Command(async () => await CambiarDatosPersonalesAsync());
            CambiarContraseniaCommand = new Command(async () => await CambiarContraseniaAsync());
            CambiarEstadoNuevaContraseniaCommand = new Command(MostrarUOcultarNuevaContrasenia);
            CambiarEstadoConfirmarNuevaContraseniaCommand = new Command(MostrarUOcultarConfirmarNuevaContrasenia);
            CerrarSesionCommand = new Command(async () => await CerrarSesion());
            // Carga los datos iniciales del usuario
            _ = CargarDatosUsuarioAsync();
        }

        // Task asincrona para controlar el estado de si puede editar sus datos (Nombre y Email)
        private async Task EstadoEdicionDeDatos()
        {
            Usuario = await _usuarioRepositorio.ObtenerUsuarioPorIdAsync(IdUsuarioActivo);
            if (Nombre == Usuario.Nombre && Email == Usuario.Email)
            {
                PuedeActualizar = false;
            }
            else
            {
                PuedeActualizar = true;
            }
        }

        // Task asincrona para cargar los datos iniciales (UsuarioActivo, NombreUsuario y EmailUsuario)
        private async Task CargarDatosUsuarioAsync()
        {
            // Capturo el id del usuario activo
            IdUsuarioActivo = _sesionService.ObtenerSesion();
            // Si no hay usuario activo salgo y muestro el Login
            if (IdUsuarioActivo <= 0)
            {
                await Shell.Current.GoToAsync("//Login");
                return;
            }
            // Capturo el usuario y cargo los datos iniciales para el VM
            Usuario = await _usuarioRepositorio.ObtenerUsuarioPorIdAsync(IdUsuarioActivo);
            Nombre = Usuario.Nombre;
            Email = Usuario.Email;
        }

        // Task asincrona para cambiar datos personales (Nombre-Email)
        private async Task CambiarDatosPersonalesAsync()
        {

            IsBusy = true;
            // Validaciones de campos
            ErrorNuevoNombre = _validacionService.ValidarNombreCompleto(Nombre);
            ErrorNuevoEmail = _validacionService.ValidarEmail(Email);
            HayErrorEnNuevoNombre = !string.IsNullOrEmpty(ErrorNuevoNombre);
            HayErrorEnNuevoEmail = !string.IsNullOrEmpty(ErrorNuevoEmail);

            // Verifica si hay errores
            if (HayErrorEnNuevoNombre || HayErrorEnNuevoEmail)
            {
                IsBusy = false;
                return;
            }


            // Validacion de si un usuario ya existe con ese email
            if (Email != Usuario.Email && await _usuarioRepositorio.ExisteUsuarioConEmailAsync(Email))
            {
                IsBusy = false;
                ErrorNuevoEmail = "Ya hay un usuario registrado con ese email.";
                HayErrorEnNuevoEmail = !string.IsNullOrEmpty(ErrorNuevoEmail);
                return;
            }

            // Actualiza los datos del usuario en la DB
            await _usuarioRepositorio.ActualizarNombreUsuarioAsync(IdUsuarioActivo, Nombre);
            await _usuarioRepositorio.ActualizarEmailUsuarioAsync(IdUsuarioActivo, Email);
            await Task.Delay(500);
            // Actualiza PuedeActualizar y muestra toast de exito
            PuedeActualizar = false;
            var toast = Toast.Make("Actualizaste tus datos con éxito.", ToastDuration.Short);
            await toast.Show();
            IsBusy = false;
        }

        // Task asincrona para cambiar contraseña
        private async Task CambiarContraseniaAsync()
        {
            IsBusy = true;
            // Validaciones de campos
            ErrorNuevaContrasenia = _validacionService.ValidarContrasenia(NuevaContrasenia);
            ErrorConfirmarNuevaContrasenia = _validacionService.ValidarConfirmarContrasenia(NuevaContrasenia, ConfirmarNuevaContrasenia);
            HayErrorEnNuevaContrasenia = !string.IsNullOrWhiteSpace(ErrorNuevaContrasenia);
            HayErrorEnConfirmarNuevaContrasenia = !string.IsNullOrWhiteSpace(ErrorConfirmarNuevaContrasenia);

            // Verifica si hay errores. Si hay sale de la funcion, sino sigue el update
            if (HayErrorEnNuevaContrasenia || HayErrorEnConfirmarNuevaContrasenia)
            {
                IsBusy = false;
                return;
            }

            //Actualiza la contraseña del usuario en la DB
            await _usuarioRepositorio.ActualizarContraseniaUsuarioAsync(IdUsuarioActivo, NuevaContrasenia);
            await Task.Delay(500);
            // Muestra toast de éxito
            var toast = Toast.Make("Actualizaste tu contraseña con éxito.", ToastDuration.Short);
            await toast.Show();
            IsBusy = false;
        }

        //Metodo para mostrar/ocultar la nueva contraseña
        private void MostrarUOcultarNuevaContrasenia()
        {
            var resultado = _estadoContraseniaService.CambiarEstadoContrasenia(NuevaContraseniaOculta, IconoNuevaContrasenia);
            NuevaContraseniaOculta = resultado.contraseniaOculta;
            IconoNuevaContrasenia = resultado.iconoContrasenia;
        }

        //Metodo para mostrar/ocultar la confirmacion de la nueva contraseña
        private void MostrarUOcultarConfirmarNuevaContrasenia()
        {
            var resultado = _estadoContraseniaService.CambiarEstadoContrasenia(ConfirmarNuevaContraseniaOculta, IconoConfirmarNuevaContrasenia);
            ConfirmarNuevaContraseniaOculta = resultado.contraseniaOculta;
            IconoConfirmarNuevaContrasenia = resultado.iconoContrasenia;
        }

        //Metodo para cerrar sesion y navegar a login
        private async Task CerrarSesion()
        {
            // Borro el IdUsuario de preferences y navego a Login
            _sesionService.CerrarSesion();
            await Shell.Current.GoToAsync("//Login");
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
