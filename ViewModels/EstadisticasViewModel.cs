using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TrainiumNeon.Data.Repositories;
using TrainiumNeon.Models;
using TrainiumNeon.Views;

namespace TrainiumNeon.ViewModels
{
    public class EstadisticasViewModel : INotifyPropertyChanged
    {
        // Repositorios
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
                    _ = ActualizarEjerciciosAsync();
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
                    _ = ActualizarEjerciciosAsync();
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

        //Comando
        public ICommand VerDetallesEjercicioCommand { get; }

        // Constructor
        public EstadisticasViewModel(IEjercicioRepositorio ejercicioRepositorio, IGrupoMuscularRepositorio grupoMuscularRepositorio)
        {
            // Inicializan servicios por DI
            _ejercicioRepositorio = ejercicioRepositorio;
            _grupoMuscularRepositorio = grupoMuscularRepositorio;
            // Inicializa comando
            VerDetallesEjercicioCommand = new Command<EjercicioModel>(async (ejercicio) => await NavegarDetallesEjercicioAsync(ejercicio));
        }

        //Task asincrona para cargar datos iniciales (Grupos musculares y Ejercicios)
        public async Task InicializarAsync()
        {
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
            GrupoMuscularSeleccionado = GruposMusculares[0];

        }

        // Task asincrona para navegar a detalles de ejercicio
        private async Task NavegarDetallesEjercicioAsync(EjercicioModel ejercicio)
        {
            await Shell.Current.GoToAsync($"{nameof(DetalleEjercicio)}?idEjercicio={ejercicio.Id}");
        }

        // Task asincrona para cargar ejercicios desde API
        private async Task CargarEjerciciosAsync()
        {
            try
            {
                // Indica que se esta cargando y muestra el spinner
                IsBusy = true;
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
            catch (Exception ex)
            {
                // Mostrar mensaje de error al usuario
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
            try
            {
                // Muestra el spiner de carga, carga los ejercicios 
                IsBusy = true;
                var listaEjercicios = await _ejercicioRepositorio.OrdenarEjerciciosAsync(MetodoOrdenamientoSeleccionado, GrupoMuscularSeleccionado.Id);
                Ejercicios = new ObservableCollection<EjercicioModel>(listaEjercicios);
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error al usuario
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                // Al terminar deja de mostrar el spinner
                IsBusy = false;
            }
        }

        // Task asincrona para Actualizar datos al cambiar el grupo muscular seleccionado
        private async Task ActualizarEjerciciosAsync()
        {
            if(GrupoMuscularSeleccionado != null)
            {
                if (!string.IsNullOrWhiteSpace(MetodoOrdenamientoSeleccionado))
                {
                    await OrdenarEjerciciosAsync();
                }
                else
                {
                    await CargarEjerciciosAsync();
                }
            }
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
