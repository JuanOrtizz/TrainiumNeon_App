using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TrainiumNeon.Data.Repositories;
using TrainiumNeon.Services;

namespace TrainiumNeon.ViewModels
{
    public class RegistroViewModel : INotifyPropertyChanged
    {
        // Servicios y repositorio
        private readonly IValidacionService _validacionService;
        private readonly IEstadoContraseniaService _estadoContraseniaService;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        // Propiedades privadas
        private string _nombre = string.Empty;
        private string _email = string.Empty;
        private string _contrasenia = string.Empty;
        private string _confirmarContrasenia = string.Empty;
        private string _errorNombre = string.Empty;
        private string _errorEmail = string.Empty;
        private string _errorContrasenia = string.Empty;
        private string _errorConfirmarContrasenia = string.Empty;
        private bool _hayErrorEnNombre;
        private bool _hayErrorEnEmail;
        private bool _hayErrorEnContrasenia;
        private bool _hayErrorEnConfirmarContrasenia;
        private bool _contraseniaOculta = true;
        private bool _confirmarContraseniaOculta = true;
        private string _iconoContrasenia = "ver_contrasenia.png";
        private string _iconoConfirmarContrasenia = "ver_contrasenia.png";
        private bool _isBusy;

        // Propiedades publicas
        public string Nombre
        {
            get => _nombre;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_nombre != nuevoValor)
                {
                    _nombre = nuevoValor;
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
                    OnPropertyChanged();
                }
            }
        }
        public string Contrasenia
        {
            get => _contrasenia;
            set
            {
                var nuevoValor = value.Trim() ?? string.Empty;
                if (_contrasenia != nuevoValor)
                {
                    _contrasenia = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ConfirmarContrasenia
        {
            get => _confirmarContrasenia;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_confirmarContrasenia != nuevoValor)
                {
                    _confirmarContrasenia = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorNombre
        {
            get => _errorNombre;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_errorNombre != nuevoValor)
                {
                    _errorNombre = nuevoValor;
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
                if (_errorContrasenia != nuevoValor)
                {
                    _errorContrasenia = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorConfirmarContrasenia
        {
            get => _errorConfirmarContrasenia;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_errorConfirmarContrasenia != nuevoValor)
                {
                    _errorConfirmarContrasenia = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public bool HayErrorEnNombre
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
        public bool HayErrorEnConfirmarContrasenia
        {
            get => _hayErrorEnConfirmarContrasenia;
            set
            {
                if (_hayErrorEnConfirmarContrasenia != value)
                {
                    _hayErrorEnConfirmarContrasenia = value;
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
        public bool ConfirmarContraseniaOculta
        {
            get => _confirmarContraseniaOculta;
            set
            {
                if (_confirmarContraseniaOculta != value)
                {
                    _confirmarContraseniaOculta = value;
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
        public string IconoConfirmarContrasenia
        {
            get => _iconoConfirmarContrasenia;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_iconoConfirmarContrasenia != nuevoValor)
                {
                    _iconoConfirmarContrasenia = nuevoValor;
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
        public ICommand RegistrarseCommand { get; }
        public ICommand IniciarSesionCommand { get; }
        public ICommand CambiarEstadoContraseniaCommand { get; }
        public ICommand CambiarEstadoConfirmarContraseniaCommand { get; }

        // Constructor
        public RegistroViewModel(IValidacionService validacionService, IEstadoContraseniaService estadoContraseniaService, IUsuarioRepositorio usuarioRepositorio)
        {
            // Inicializan servicios y repositorio por DI
            _validacionService = validacionService;
            _estadoContraseniaService = estadoContraseniaService;
            _usuarioRepositorio = usuarioRepositorio;
            // Inicializan comandos
            RegistrarseCommand = new Command(async () => await RegistroAsync());
            IniciarSesionCommand = new Command(async () => await Shell.Current.GoToAsync("//Login"));
            CambiarEstadoContraseniaCommand = new Command(MostrarUOcultarContrasenia);
            CambiarEstadoConfirmarContraseniaCommand = new Command(MostrarUOcultarConfirmarContrasenia);
        }

        //Task para registrar usuario en la DB 
        private async Task RegistroAsync()
        {
            // Muestro el spinner de carga
            IsBusy = true;
            // Validaciones de campos 
            ErrorNombre = _validacionService.ValidarNombreCompleto(Nombre);
            ErrorEmail = _validacionService.ValidarEmail(Email);
            ErrorContrasenia = _validacionService.ValidarContrasenia(Contrasenia);
            ErrorConfirmarContrasenia = _validacionService.ValidarConfirmarContrasenia(Contrasenia, ConfirmarContrasenia);
            HayErrorEnNombre = !string.IsNullOrEmpty(ErrorNombre);
            HayErrorEnEmail = !string.IsNullOrEmpty(ErrorEmail);
            HayErrorEnContrasenia= !string.IsNullOrEmpty(ErrorContrasenia);
            HayErrorEnConfirmarContrasenia = !string.IsNullOrEmpty(ErrorConfirmarContrasenia);

            // Verifica si hay errores. Si hay sale de la funcion, sino sigue el registro
            if (HayErrorEnNombre || HayErrorEnEmail || HayErrorEnContrasenia || HayErrorEnConfirmarContrasenia)
            {
                IsBusy = false;
                return;
            }

            //Validacion de si un usuario ya existe con ese email
            if (await _usuarioRepositorio.ExisteUsuarioConEmailAsync(Email))
            {
                IsBusy = false;
                ErrorEmail = "Ya hay un usuario registrado con ese email.";
                HayErrorEnEmail = true;
                return;
            }

            //Registro en DB
            var exito = await _usuarioRepositorio.RegistrarUsuarioAsync(Nombre, Email, Contrasenia);
            await Task.Delay(500);
            if (exito)
            {
                //Envia mensaje de registro exitoso a mainpage para mostrar toast
                MessagingCenter.Send(this, "RegistroExitoso");

                //Navega a inicio de sesion para que se logee en la app
                await Shell.Current.GoToAsync("//Login");
            }
        }

        //Metodo para mostrar/ocultar la contraseña
        private void MostrarUOcultarContrasenia()
        {
            // Capturo el estado actual de la contraseña y el icono a mostrar
            var resultado = _estadoContraseniaService.CambiarEstadoContrasenia(ContraseniaOculta, IconoContrasenia);
            // Actualizo el estado e icono de la contraseña
            ContraseniaOculta = resultado.contraseniaOculta;
            IconoContrasenia = resultado.iconoContrasenia;
        }

        //Metodo para mostrar/ocultar la confirmacion de la contraseña
        private void MostrarUOcultarConfirmarContrasenia()
        {
            // Capturo el estado actual de la contraseña y el icono a mostrar
            var resultado = _estadoContraseniaService.CambiarEstadoContrasenia(ConfirmarContraseniaOculta, IconoConfirmarContrasenia);
            // Actualizo el estado e icono de la contraseña
            ConfirmarContraseniaOculta = resultado.contraseniaOculta;
            IconoConfirmarContrasenia = resultado.iconoContrasenia;
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
