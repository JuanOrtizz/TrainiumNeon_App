using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.LocalNotification;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TrainiumNeon.Data.Repositories;
using TrainiumNeon.Messages;
using TrainiumNeon.Models;
using TrainiumNeon.Services;

namespace TrainiumNeon.ViewModels
{
    public class PerfilViewModel : INotifyPropertyChanged
    {
        // Constante privada para llave de Prefereneces (Notificaciones)
        private const string NotificacionesKey = "NotificacionesActivas";

        // Servicios y repositorios 
        private readonly IValidacionService _validacionService;
        private readonly ISesionService _sesionService;
        private readonly IPermisosService _permisosService;
        private readonly INotificacionService _notificacionService;
        private readonly IEstadoContraseniaService _estadoContraseniaService;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        // Propiedades privadas
        private int _idUsuarioActivo;
        private UsuarioModel _usuario;
        private bool _notificacionesActivas;
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
        public bool NotificacionesActivas
        {
            get => _notificacionesActivas;
            set
            {
                if (_notificacionesActivas != value)
                {
                    _notificacionesActivas = value;
                    _ = ProcesarCambioNotificacionesAsync();
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
        public PerfilViewModel(IValidacionService validacionService, IEstadoContraseniaService estadoContraseniaService, IPermisosService permisosService, INotificacionService notificacionService, IUsuarioRepositorio usuarioRepositorio, ISesionService sesionService)
        {
            // Inicializan servicios y repositorios por DI
            _validacionService = validacionService;
            _estadoContraseniaService = estadoContraseniaService;
            _permisosService = permisosService;
            _notificacionService = notificacionService;
            _usuarioRepositorio = usuarioRepositorio;
            _sesionService = sesionService;
            // Inicializan comandos
            CambiarDatosPersonalesCommand = new Command(async () => await CambiarDatosPersonalesAsync());
            CambiarContraseniaCommand = new Command(async () => await CambiarContraseniaAsync());
            CambiarEstadoNuevaContraseniaCommand = new Command(MostrarUOcultarNuevaContrasenia);
            CambiarEstadoConfirmarNuevaContraseniaCommand = new Command(MostrarUOcultarConfirmarNuevaContrasenia);
            CerrarSesionCommand = new Command(async () => await CerrarSesion());
            
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
        public async Task InicializarAsync()
        {
            // Capturo el id del usuario activo y si tiene activas las notificaciones
            IdUsuarioActivo = _sesionService.ObtenerSesion();
            NotificacionesActivas = Preferences.Get(NotificacionesKey, false);
            OnPropertyChanged(nameof(NotificacionesActivas));
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
            // muestro el spinner de carga
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

            //Envia mensaje de actualizacion para que se actualicen los datos en otros viewModels
            WeakReferenceMessenger.Default.Send(new UsuarioMessages.UsuarioActualizadoMessage());

            await Task.Delay(500);
            // Actualiza PuedeActualizar y muestra toast de exito
            PuedeActualizar = false;
            var toast = Toast.Make("Actualizaste tus datos con éxito.", ToastDuration.Short);
            await toast.Show();

            // oculto el spinner de carga
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

        //Task asincrona para cerrar sesion y navegar a login
        private async Task CerrarSesion()
        {
            // Borro el IdUsuario de preferences y navego a Login
            _sesionService.CerrarSesion();
            await Shell.Current.GoToAsync("//Login");
        }

        // Task asincrona para solicitar permisos de notificacion al cambiar el valor del switch
        private async Task ProcesarCambioNotificacionesAsync()
        {
            // Cargar valor guardado
            Preferences.Set(NotificacionesKey, NotificacionesActivas);
            // si es true, solicitar permisos y programar notificacion diaria
            if (NotificacionesActivas)
            {
                if (!await _permisosService.SolicitarPermisosNotificacionesAsync())
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Permisos requeridos",
                        "Tenes que permitir el permiso para que la app te envie notificaciones.",
                        "OK");

                    NotificacionesActivas = false;
                    return;
                }
                else
                {
                    // Programo la notificacion diaria para las 10 am
                    var hora = DateTime.Today.AddHours(10);
                    // Si la hora ya paso para hoy, la programo para mañana
                    if (hora < DateTime.Now)
                    {
                        hora = hora.AddDays(1);
                    }
                    // Envio notificacion diaria
                    _notificacionService.EnviarNotificacion("¡Vamos a entrenar!","No te olvides de asistir al gimnasio para obtener los mejores resultados", hora, true);
                }
            }
            else
            {
                // Cancelar todas Si el usuario las desactiva
                LocalNotificationCenter.Current.CancelAll();
            }
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
