using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrainiumNeon.Services;

namespace TrainiumNeon.ViewModels
{
    public class PerfilViewModel : INotifyPropertyChanged
    {
        // Servicio
        private readonly IValidacionService _validacionService;
        private readonly IEstadoContraseniaService _estadoContraseniaService;
        // Propiedades privadas
        private string _nuevoNombre;
        private string _nuevoEmail;
        private string _nuevaContrasenia;
        private string _confirmarNuevaContrasenia;
        private string _errorNuevoNombre;
        private string _errorNuevoEmail;
        private string _errorNuevaContrasenia;
        private string _errorConfirmarNuevaContrasenia;
        private bool _hayErrorEnNombre;
        private bool _hayErrorEnEmail;
        private bool _hayErrorEnNuevaContrasenia;
        private bool _hayErrorEnConfirmarNuevaContrasenia;
        private bool _nuevaContraseniaOculta;
        private bool _confirmarNuevaContraseniaOculta;
        private string _iconoNuevaContrasenia;
        private string _iconoConfirmarNuevaContrasenia;
        // Propiedades publicas
        public string NuevoNombre
        {
            get => _nuevoNombre;
            set
            {
                if (_nuevoNombre != value)
                {
                    _nuevoNombre = value;
                    OnPropertyChanged();
                }
            }
        }
        public string NuevoEmail
        {
            get => _nuevoEmail;
            set
            {
                if (_nuevoEmail != value)
                {
                    _nuevoEmail = value;
                    OnPropertyChanged();
                }
            }
        }
        public string NuevaContrasenia
        {
            get => _nuevaContrasenia;
            set
            {
                if (_nuevaContrasenia != value)
                {
                    _nuevaContrasenia = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ConfirmarNuevaContrasenia
        {
            get => _confirmarNuevaContrasenia;
            set
            {
                if (_confirmarNuevaContrasenia != value)
                {
                    _confirmarNuevaContrasenia = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorNuevoNombre
        {
            get => _errorNuevoNombre;
            set
            {
                if (_errorNuevoNombre != value)
                {
                    _errorNuevoNombre = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorNuevoEmail
        {
            get => _errorNuevoEmail;
            set
            {
                if (_errorNuevoEmail != value)
                {
                    _errorNuevoEmail = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorNuevaContrasenia
        {
            get => _errorNuevaContrasenia;
            set
            {
                if (_errorNuevaContrasenia != value)
                {
                    _errorNuevaContrasenia = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorConfirmarNuevaContrasenia
        {
            get => _errorConfirmarNuevaContrasenia;
            set
            {
                if (_errorConfirmarNuevaContrasenia != value)
                {
                    _errorConfirmarNuevaContrasenia = value;
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
                if (_iconoNuevaContrasenia != value)
                {
                    _iconoNuevaContrasenia = value;
                    OnPropertyChanged();
                }
            }
        }
        public string IconoConfirmarNuevaContrasenia
        {
            get => _iconoConfirmarNuevaContrasenia;
            set
            {
                if (_iconoConfirmarNuevaContrasenia != value)
                {
                    _iconoConfirmarNuevaContrasenia = value;
                    OnPropertyChanged();
                }
            }
        }

        //Comandos
        public Command CambiarDatosPersonalesCommand { get; }
        public Command CambiarContraseniaCommand { get; }
        public Command CambiarEstadoNuevaContraseniaCommand { get; }
        public Command CambiarEstadoConfirmarNuevaContraseniaCommand { get; }

        // Constructor
        public PerfilViewModel(IValidacionService validacionService, IEstadoContraseniaService estadoContraseniaService)
        {
            // Inicializa servicio por DI
            _validacionService = validacionService;
            _estadoContraseniaService = estadoContraseniaService;
            // Inicializan iconos por default
            NuevaContraseniaOculta = true;
            IconoNuevaContrasenia = "ver_contrasenia.png";
            ConfirmarNuevaContraseniaOculta = true;
            IconoConfirmarNuevaContrasenia = "ver_contrasenia.png";
            // Inicializan comandos
            CambiarDatosPersonalesCommand = new Command(CambiarDatosPersonales);
            CambiarContraseniaCommand = new Command(CambiarContrasenia);
            CambiarEstadoNuevaContraseniaCommand = new Command(MostrarUOcultarNuevaContrasenia);
            CambiarEstadoConfirmarNuevaContraseniaCommand = new Command(MostrarUOcultarConfirmarNuevaContrasenia);
        }

        // Metodo privado para cambiar datos personales (Nombre-Email)
        private void CambiarDatosPersonales()
        {
            ErrorNuevoNombre = _validacionService.ValidarNombreCompleto(NuevoNombre);
            ErrorNuevoEmail = _validacionService.ValidarEmail(NuevoEmail);
            HayErrorEnNuevoNombre = !string.IsNullOrEmpty(ErrorNuevoNombre);
            HayErrorEnNuevoEmail = !string.IsNullOrEmpty(ErrorNuevoEmail);

            if (HayErrorEnNuevoNombre || HayErrorEnNuevoNombre)
            {
                return;
            }

        }

        // Metodo privado para cambiar contraseña
        private void CambiarContrasenia()
        {
            ErrorNuevaContrasenia = _validacionService.ValidarContrasenia(NuevaContrasenia);
            ErrorConfirmarNuevaContrasenia = _validacionService.ValidarConfirmarContrasenia(NuevaContrasenia, ConfirmarNuevaContrasenia);
            HayErrorEnNuevaContrasenia = !string.IsNullOrWhiteSpace(ErrorNuevaContrasenia);
            HayErrorEnConfirmarNuevaContrasenia = !string.IsNullOrWhiteSpace(ErrorConfirmarNuevaContrasenia);

            if(HayErrorEnNuevaContrasenia || HayErrorEnConfirmarNuevaContrasenia)
            {
                return;
            }
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

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
