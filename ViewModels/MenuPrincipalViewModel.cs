using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using TrainiumNeon.Data.Repositories;
using TrainiumNeon.Messages;
using TrainiumNeon.Models;
using TrainiumNeon.Services;


namespace TrainiumNeon.ViewModels
{
    public class MenuPrincipalViewModel : INotifyPropertyChanged, IRecipient<RutinaMessages.RutinaCreadaMessage>, IRecipient<RutinaMessages.RutinaActualizadaMessage>, IRecipient<RutinaMessages.RutinaEliminadaMessage>, IRecipient<RutinaMessages.RutinaSeleccionadaActualizadaMessage>, IRecipient<UsuarioMessages.UsuarioActualizadoMessage>
    {
        //Servicios y repositorios
        private readonly ISesionService _sesionService;
        private readonly IEjerciciosSyncService _ejerciciosSyncService;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IRutinaRepositorio _rutinaRepositorio;
        private readonly IDiaRepositorio _diaRepositorio;
        private readonly IEjercicioDiaRepositorio _ejercicioDiaRepositorio;
        private readonly IEjercicioRepositorio _ejercicioRepositorio;
        // Propiedades privadas
        private int _idUsuarioActivo;
        private string _nombreUsuario = string.Empty;
        private string _rutinaSeleccionadaNombre = string.Empty;
        private ObservableCollection<EjercicioDiaModel> _ejerciciosDelDia = new ObservableCollection<EjercicioDiaModel>();
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
        public string NombreUsuario
        {
            get => _nombreUsuario;
            set
            {
                var nuevoValor = value.Trim() ?? string.Empty;
                if (_nombreUsuario != nuevoValor)
                {
                    _nombreUsuario = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string RutinaSeleccionadaNombre
        {
            get => _rutinaSeleccionadaNombre;
            set
            {
                var nuevoValor = value.Trim() ?? string.Empty;
                if (_rutinaSeleccionadaNombre != nuevoValor)
                {
                    _rutinaSeleccionadaNombre = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<EjercicioDiaModel> EjerciciosDelDia
        {
            get => _ejerciciosDelDia;
            set
            {
                if (_ejerciciosDelDia != value)
                {
                    _ejerciciosDelDia = value;
                    OnPropertyChanged();
                }
            }
        }

        public MenuPrincipalViewModel(ISesionService sesionService, IUsuarioRepositorio usuarioRepositorio, IRutinaRepositorio rutinaRepositorio, IDiaRepositorio diaRepositorio, IEjercicioDiaRepositorio ejercicioDiaRepositorio,IEjerciciosSyncService ejerciciosSyncService, IEjercicioRepositorio ejercicioRepositorio)
        {
            // Inicializan los servicios y repositorios por DI
            _sesionService = sesionService;
            _ejerciciosSyncService = ejerciciosSyncService;
            _usuarioRepositorio = usuarioRepositorio;
            _rutinaRepositorio = rutinaRepositorio;
            _diaRepositorio = diaRepositorio;
            _ejercicioDiaRepositorio = ejercicioDiaRepositorio;
            _ejercicioRepositorio = ejercicioRepositorio;
            // Suscripcion a mensajeria para actualizar datos al crear, actualizar o eliminar rutina y actualizar usuario
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        // Task asincrona para inicializar los datos del VM
        public async Task InicializarAsync()
        {
            // Sincroniza los ejercicios al cargar menu principal (Si hay internet)
            await _ejerciciosSyncService.SincronizarEjerciciosAsync();
            // Carga los datos iniciales (UsuarioActivo, NombreUsuario, RutinaSeleccionada y EjerciciosDelDia)
            await CargarDatosAsync();
        }

        // Task asincrona para cargar los datos iniciales (UsuarioActivo, NombreUsuario, RutinaSeleccionada y EjerciciosDelDia)
        private async Task CargarDatosAsync()
        {
            // Capturo el id del usuario activo
            IdUsuarioActivo = _sesionService.ObtenerSesion();
            // Si no hay usuario activo salgo y muestro el Login
            if (IdUsuarioActivo <= 0)
            {
                await Shell.Current.GoToAsync("//Login");
                return;
            }

            await ObtenerInformacionUsuarioAsync();

            await ObtenerInformacionRutinaAsync();
        }

        private async Task ObtenerInformacionUsuarioAsync()
        {
            //Si hay usuario activo lo obtengo para capturar su nombre
            var usuario = await _usuarioRepositorio.ObtenerUsuarioPorIdAsync(IdUsuarioActivo);
            NombreUsuario = usuario.Nombre;
        }

        private async Task ObtenerInformacionRutinaAsync()
        {
            // Obtengo la rutina seleccionada del usuario activo para capturar su nombre, si no hay rutina seleccionada muestra Ninguna
            var rutina = await _rutinaRepositorio.ObtenerRutinaSeleccionadaAsync(IdUsuarioActivo);
            if (rutina != null)
            {
                RutinaSeleccionadaNombre = rutina.Nombre;
            }
            else
            {
                RutinaSeleccionadaNombre = "Ninguna.";
                return;
            }

            // Capturo el nombre del dia actual en español
            string diaNombre = CultureInfo.GetCultureInfo("es-ES").TextInfo.ToTitleCase(DateTime.Now.ToString("dddd", new CultureInfo("es-ES")));
            // Obtengo el dia correspondiente en la rutina
            var dia = await _diaRepositorio.ObtenerDiaPorNombreAsync(rutina.Id, diaNombre);
            // Obtengo los ejercicios del dia
            var ejerciciosDia = await _ejercicioDiaRepositorio.ObtenerEjerciciosPorDiaAsync(dia.Id);
            // Recorro los ejercicios por dia para obtener el ejercicio y mostrar su nombre
            foreach (var ed in ejerciciosDia)
            {
                ed.Ejercicio = await _ejercicioRepositorio.ObtenerEjercicioPorIdAsync(ed.IdEjercicio);
            }

            EjerciciosDelDia = new ObservableCollection<EjercicioDiaModel>(ejerciciosDia);
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        // Implementacion de IRecipient para recibir mensaje de registro exitoso
        public async void Receive(RutinaMessages.RutinaCreadaMessage message)
        {
            await ObtenerInformacionRutinaAsync() ;
        }

        public async void Receive(RutinaMessages.RutinaActualizadaMessage message)
        {
            await ObtenerInformacionRutinaAsync();
        }

        public async void Receive(RutinaMessages.RutinaEliminadaMessage message)
        {
            await ObtenerInformacionRutinaAsync();
        }


        public async void Receive(RutinaMessages.RutinaSeleccionadaActualizadaMessage message)
        {
            await ObtenerInformacionRutinaAsync();
        }

        public async void Receive(UsuarioMessages.UsuarioActualizadoMessage message)
        {
            await ObtenerInformacionUsuarioAsync();
        }
    }
}
