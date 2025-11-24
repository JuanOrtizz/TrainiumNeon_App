using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TrainiumNeon.Data.Repositories;
using TrainiumNeon.Models;
using TrainiumNeon.Services;
using TrainiumNeon.Views;

namespace TrainiumNeon.ViewModels
{
    public class RutinasViewModel : INotifyPropertyChanged
    {
        // Servicio y repositorio
        private readonly ISesionService _sesionService;
        private readonly IRutinaRepositorio _rutinaRepositorio;

        //Propiedades privadas
        private int _idUsuarioActivo;
        private ObservableCollection<RutinaModel> _rutinas;
        private RutinaModel _rutinaSeleccionada;

        //Propiedades Publicas
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
        public ObservableCollection<RutinaModel> Rutinas
        {
            get => _rutinas;
            set
            {
                if (_rutinas != value)
                {
                    _rutinas = value;
                    OnPropertyChanged();
                }
            }
        }
        public RutinaModel RutinaSeleccionada
        {
            get => _rutinaSeleccionada;
            set
            {
                if(_rutinaSeleccionada != value)
                {
                    _rutinaSeleccionada = value;
                    _ = MarcarRutinaComoSeleccionada();
                    OnPropertyChanged();
                }
            }
        }

        // Comando 
        public ICommand AgregarRutinaCommand { get; }
        public ICommand EditarRutinaCommand { get; }

        // Constructor
        public RutinasViewModel(ISesionService sesionService, IRutinaRepositorio rutinaRepositorio)
        {
            // Inicializa servicio y repositorio por DI
            _sesionService = sesionService;
            _rutinaRepositorio = rutinaRepositorio;
            // InicializaN comandoS
            AgregarRutinaCommand = new Command(async () => await NavegarAgregarRutina());
            EditarRutinaCommand = new Command<int>(async (idRutina) => await NavegarEditarRutina(idRutina));
            //Cargo los datos
            _ = CargarRutinasAsync();
        }

        // Task asincrona para cargar las rutinas al iniciar
        private async Task CargarRutinasAsync()
        {
            // Capturo el Id del usuario activo (logeado)
            IdUsuarioActivo = _sesionService.ObtenerSesion();
            // Inicializo la lista
            IReadOnlyList<RutinaModel> listaRutinas;
            // Obtengo las rutinas del usuario
            listaRutinas = await _rutinaRepositorio.ObtenerRutinasPorUsuarioAsync(IdUsuarioActivo);
            Rutinas = new ObservableCollection<RutinaModel>(listaRutinas);
            // cargo la rutina seleccionada
            var rutinaSeleccionada = await _rutinaRepositorio.ObtenerRutinaSeleccionadaAsync(IdUsuarioActivo);
            RutinaSeleccionada = Rutinas.FirstOrDefault(r => r.Id == rutinaSeleccionada.Id);
        }

        // Task asincrona para marcar una rutina como seleccionada
        private async Task MarcarRutinaComoSeleccionada()
        {
            await _rutinaRepositorio.MarcarRutinaSeleccionadaAsync(RutinaSeleccionada.Id);
        }

        // Task asincrona para navegar a la pagina de agregar rutina
        private async Task NavegarAgregarRutina()
        {
            await Shell.Current.GoToAsync($"{nameof(AgregarEditarRutina)}?accion=Agregar");
        }

        // Task asincrona para navegar a la pagina de editar rutina
        private async Task NavegarEditarRutina(int idRutina)
        {
            await Shell.Current.GoToAsync($"{nameof(AgregarEditarRutina)}?accion=Editar&idRutina={idRutina}");
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
