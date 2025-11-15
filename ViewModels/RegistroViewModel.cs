using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrainiumNeon.Services;

namespace TrainiumNeon.ViewModels
{
    public class RegistroViewModel : INotifyPropertyChanged
    {
        // Servicios
        private readonly IValidacionService _validacionService;
        private readonly IEstadoContraseniaService _estadoContraseniaService;
        // Propiedades privadas
        private INavigation _navigation;
        private string _nombre;
        private string _email;
        private string _contrasenia;
        private string _confirmarContrasenia;
        private string _errorNombre;
        private string _errorEmail;
        private string _errorContrasenia;
        private string _errorConfirmarContrasenia;
        private bool _hayErrorEnNombre;
        private bool _hayErrorEnEmail;
        private bool _hayErrorEnContrasenia;
        private bool _hayErrorEnConfirmarContrasenia;
        private bool _contraseniaOculta;
        private bool _confirmarContraseniaOculta;
        private string _iconoContrasenia;
        private string _iconoConfirmarContrasenia;
        // Propiedades publicas
        public string Nombre
        {
            get => _nombre;
            set
            {
                if (_nombre != value)
                {
                    _nombre = value;
                    OnPropertyChanged();
                }
            }

        }
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
        public string ConfirmarContrasenia
        {
            get => _confirmarContrasenia;
            set
            {
                if (_confirmarContrasenia != value)
                {
                    _confirmarContrasenia = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorNombre
        {
            get => _errorNombre;
            set
            {
                if (_errorNombre != value)
                {
                    _errorNombre = value;
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
        public string ErrorConfirmarContrasenia
        {
            get => _errorConfirmarContrasenia;
            set
            {
                if (_errorConfirmarContrasenia != value)
                {
                    _errorConfirmarContrasenia = value;
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
                if (_iconoContrasenia != value)
                {
                    _iconoContrasenia = value;
                    OnPropertyChanged();
                }
            }
        }
        public string IconoConfirmarContrasenia
        {
            get => _iconoConfirmarContrasenia;
            set
            {
                if (_iconoConfirmarContrasenia != value)
                {
                    _iconoConfirmarContrasenia = value;
                    OnPropertyChanged();
                }
            }
        }

        // Comandos
        public Command RegistrarseCommand { get; }
        public Command IniciarSesionCommand { get; }
        public Command CambiarEstadoContraseniaCommand { get; }
        public Command CambiarEstadoConfirmarContraseniaCommand { get; }


        // Constructor
        public RegistroViewModel(IValidacionService validacionService, IEstadoContraseniaService estadoContraseniaService)
        {
            // Inicializan servicios por DI
            _validacionService = validacionService;
            _estadoContraseniaService = estadoContraseniaService;
            // Inicializan iconos por default
            ContraseniaOculta = true;
            IconoContrasenia = "ver_contrasenia.png";
            ConfirmarContraseniaOculta = true;
            IconoConfirmarContrasenia = "ver_contrasenia.png";
            // Inicializan comandos
            RegistrarseCommand = new Command(async () => await Registrarse());
            IniciarSesionCommand = new Command(async () => await _navigation.PopAsync());
            CambiarEstadoContraseniaCommand = new Command(MostrarUOcultarContrasenia);
            CambiarEstadoConfirmarContraseniaCommand = new Command(MostrarUOcultarConfirmarContrasenia);
        }

        private async Task Registrarse()
        {
            //logica de capturar, validar y mostrar errores en la UI.
            ErrorNombre = _validacionService.ValidarNombreCompleto(Nombre);
            ErrorEmail = _validacionService.ValidarEmail(Email);
            ErrorContrasenia = _validacionService.ValidarContrasenia(Contrasenia);
            ErrorConfirmarContrasenia = _validacionService.ValidarConfirmarContrasenia(Contrasenia, ConfirmarContrasenia);
            HayErrorEnNombre = !string.IsNullOrEmpty(ErrorNombre);
            HayErrorEnEmail = !string.IsNullOrEmpty(ErrorEmail);
            HayErrorEnContrasenia= !string.IsNullOrEmpty(ErrorContrasenia);
            HayErrorEnConfirmarContrasenia = !string.IsNullOrEmpty(ErrorConfirmarContrasenia);

            if (HayErrorEnNombre || HayErrorEnEmail || HayErrorEnContrasenia || HayErrorEnConfirmarContrasenia)
            {
                return;
            }

            //Registro en DB (Se agrega en el futuro)

            //Envia mensaje de registro exitoso a mainpage para mostrar toast
            MessagingCenter.Send(this, "RegistroExitoso");

            //Navega a inicio de sesion para que se logee en la app
            await _navigation.PopAsync();
        }

        //Metodo para mostrar/ocultar la contraseña
        private void MostrarUOcultarContrasenia()
        {
            var resultado = _estadoContraseniaService.CambiarEstadoContrasenia(ContraseniaOculta, IconoContrasenia);
            ContraseniaOculta = resultado.contraseniaOculta;
            IconoContrasenia = resultado.iconoContrasenia;
        }

        //Metodo para mostrar/ocultar la confirmacion de la contraseña
        private void MostrarUOcultarConfirmarContrasenia()
        {
            var resultado = _estadoContraseniaService.CambiarEstadoContrasenia(ConfirmarContraseniaOculta, IconoConfirmarContrasenia);
            ConfirmarContraseniaOculta = resultado.contraseniaOculta;
            IconoConfirmarContrasenia = resultado.iconoContrasenia;
        }

        // Metodo para inicializar la navegacion desde Code-Behind
        public void SetNavigation(INavigation navigation) => _navigation = navigation;

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
