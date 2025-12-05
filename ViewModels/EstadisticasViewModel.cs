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
    public class EstadisticasViewModel : INotifyPropertyChanged
    {
        // Servicio y Repositorios
        private readonly IDisplayAlertService _displayAlertService;
        private readonly IEjercicioRepositorio _ejercicioRepositorio;
        private readonly IGrupoMuscularRepositorio _grupoMuscularRepositorio;

        // Propiedades privadas
        private string _metodoOrdenamientoSeleccionado = string.Empty;
        private ObservableCollection<EjercicioModel> _ejercicios = new ObservableCollection<EjercicioModel>();
        private ObservableCollection<GrupoMuscularModel> _gruposMusculares = new ObservableCollection<GrupoMuscularModel>();
        private GrupoMuscularModel _grupoMuscularSeleccionado = new GrupoMuscularModel{Id = 0, Nombre = "Todos"};
        private bool _isBusy;

        // Propiedades publicas
        public string MetodoOrdenamientoSeleccionado
        {
            get => _metodoOrdenamientoSeleccionado;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_metodoOrdenamientoSeleccionado != nuevoValor)
                {
                    _metodoOrdenamientoSeleccionado = nuevoValor;
                    OnPropertyChanged();
                    ActualizarEjerciciosCommand.Execute(null);
                }
            }
        }
        public ObservableCollection<EjercicioModel> Ejercicios
        {
            get => _ejercicios;
            set
            {
                if (_ejercicios != value)
                {
                    _ejercicios = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<GrupoMuscularModel> GruposMusculares
        {
            get => _gruposMusculares;
            set
            {
                if (_gruposMusculares != value)
                {
                    _gruposMusculares = value;
                    OnPropertyChanged();
                }
            }
        }
        public GrupoMuscularModel GrupoMuscularSeleccionado
        {
            get => _grupoMuscularSeleccionado;
            set
            {
                if(_grupoMuscularSeleccionado != value)
                {
                    _grupoMuscularSeleccionado = value;
                    OnPropertyChanged();
                    ActualizarEjerciciosCommand.Execute(null);
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

        //Comandos
        public ICommand ActualizarEjerciciosCommand { get; }
        public ICommand VerDetallesEjercicioCommand { get; }

        // Constructor
        public EstadisticasViewModel(IDisplayAlertService displayAlertService, IEjercicioRepositorio ejercicioRepositorio, IGrupoMuscularRepositorio grupoMuscularRepositorio)
        {
            // Inicializan servicio y repositorios por DI
            _displayAlertService = displayAlertService;
            _ejercicioRepositorio = ejercicioRepositorio;
            _grupoMuscularRepositorio = grupoMuscularRepositorio;
            // Inicializa comando
            ActualizarEjerciciosCommand = new Command(async () => await ActualizarEjerciciosAsync());
            VerDetallesEjercicioCommand = new Command<EjercicioModel>(async (ejercicio) => await NavegarDetallesEjercicioAsync(ejercicio));
        }

        //Task asincrona para cargar datos iniciales (Grupos musculares y Ejercicios)
        public async Task InicializarAsync()
        {
            try
            {
                // Muestra el spinner de carga
                IsBusy = true;
                // Obtengo los grupos musculares
                var listaGruposMusculares = await _grupoMuscularRepositorio.ObtenerTodoGruposMuscularesAsync();
                // Lo asigno a la propiedad
                GruposMusculares = new ObservableCollection<GrupoMuscularModel>(listaGruposMusculares);
                // Agrego "Todos" con Id 0 en grupos musculares
                GruposMusculares.Insert(0, new GrupoMuscularModel
                {
                    Id = 0,
                    Nombre = "Todos"
                });
                // Selecciono el grupo muscular "Todos" por defecto
                GrupoMuscularSeleccionado = GruposMusculares[0];
            }
            catch (Exception)
            {
                await _displayAlertService.MostrarAlertAsync("Error", "No se pudieron cargar los datos iniciales. Intentá mas tarde", "OK");
            }
            finally
            {
                // Oculto el spinner
                IsBusy = false;
            }        
        }

        // Task asincrona para Actualizar datos al cambiar el grupo muscular seleccionado
        private async Task ActualizarEjerciciosAsync()
        {
            try
            {
                // Muestra el spiner de carga, carga los ejercicios 
                IsBusy = true;
                // Verifica que haya un grupo muscular seleccionado
                if (GrupoMuscularSeleccionado != null)
                {
                    // Si hay un metodo de ordenamiento seleccionado, ordena los ejercicios
                    if (!string.IsNullOrWhiteSpace(MetodoOrdenamientoSeleccionado))
                    {
                        await OrdenarEjerciciosAsync();
                    }
                    // Si no hay metodo de ordenamiento seleccionado, carga los ejercicios normalmente
                    else
                    {
                        await CargarEjerciciosAsync();
                    }
                }
            }
            catch (Exception)
            {
                await _displayAlertService.MostrarAlertAsync("Error", "No se pudieron cargar los ejercicios", "OK");
            }
            finally
            {
                // Al terminar deja de mostrar el spinner
                IsBusy = false;
            }
        }

        // Task asincrona para ordenar los ejercicios
        private async Task OrdenarEjerciciosAsync()
        {
            var listaEjercicios = await _ejercicioRepositorio.OrdenarEjerciciosAsync(MetodoOrdenamientoSeleccionado, GrupoMuscularSeleccionado.Id);
            Ejercicios = new ObservableCollection<EjercicioModel>(listaEjercicios);
        }

        // Task asincrona para cargar ejercicios desde API
        private async Task CargarEjerciciosAsync()
        {
            // Declaro una lista de ejercicios
            IReadOnlyList<EjercicioModel> listaEjercicios;
            // Obtengo la lista por grupo muscular seleccionado
            if (GrupoMuscularSeleccionado.Id == 0)
            {
                listaEjercicios = await _ejercicioRepositorio.ObtenerTodosLosEjerciciosAsync();
            }
            else
            {
                listaEjercicios = await _ejercicioRepositorio.ObtenerEjerciciosPorGrupoMuscularAsync(GrupoMuscularSeleccionado.Id);
            }
            // Asigna la lista obtenida a la coleccion observable
            Ejercicios = new ObservableCollection<EjercicioModel>(listaEjercicios);
        }

        // Task asincrona para navegar a detalles de ejercicio
        private async Task NavegarDetallesEjercicioAsync(EjercicioModel ejercicio)
        {
            await Shell.Current.GoToAsync($"{nameof(DetalleEjercicio)}?idEjercicio={ejercicio.Id}");
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
