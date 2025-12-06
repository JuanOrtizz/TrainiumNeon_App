using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TrainiumNeon.Data.Repositories;
using TrainiumNeon.Messages;
using TrainiumNeon.Models;
using TrainiumNeon.Services;

namespace TrainiumNeon.ViewModels
{
    [QueryProperty(nameof(AccionTitulo), "accion")]
    [QueryProperty(nameof(IdRutina), "idRutina")]
    [QueryProperty(nameof(IdEjercicioDia), "idEjercicioDia")]
    public class AgregarEditarEjercicioRutinaViewModel: INotifyPropertyChanged
    {
        //Servicio y repositorios
        private readonly IValidacionService _validacionService;
        private readonly IDisplayAlertService _displayAlertService;
        private readonly IEjercicioRepositorio _ejercicioRepositorio;
        private readonly IRutinaRepositorio _rutinaRepositorio;
        private readonly IDiaRepositorio _diaRepositorio;
        private readonly IEjercicioDiaRepositorio _ejercicioDiaRepositorio;
        private readonly IGrupoMuscularRepositorio _grupoMuscularRepositorio;

        // Propiedades privadas
        private string _accionTitulo = string.Empty;
        private int _idRutina;
        private int _idEjercicioDia;
        private RutinaModel? _rutina;
        private EjercicioDiaModel? _ejercicioDia;
        private ObservableCollection<DiaModel> _diasSemana = new ObservableCollection<DiaModel>();
        private DiaModel? _diaSeleccionado;
        private ObservableCollection<GrupoMuscularModel> _gruposMusculares = new ObservableCollection<GrupoMuscularModel>();
        private GrupoMuscularModel? _grupoMuscularSeleccionado;
        private ObservableCollection<EjercicioModel> _ejercicios = new ObservableCollection<EjercicioModel>();
        private EjercicioModel? _ejercicioSeleccionado;
        private string _series = string.Empty;
        private string _repeticiones = string.Empty;
        private string _errorEjercicioSeleccionado = string.Empty;
        private string _errorSeries = string.Empty;
        private string _errorRepeticiones = string.Empty;
        private bool _hayErrorEnEjercicioSeleccionado;
        private bool _hayErrorEnSeries;
        private bool _hayErrorEnRepeticiones;
        private bool _isBusy;


        // Propiedad de lectura
        public bool EsEditar => AccionTitulo == "Editar";

        //Propiedades publicas
        public string AccionTitulo
        {
            get => _accionTitulo;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_accionTitulo != nuevoValor)
                {
                    _accionTitulo = nuevoValor;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(EsEditar));
                }
            }
        }
        public int IdRutina
        {
            get => _idRutina;
            set
            {
                if (_idRutina != value)
                {
                    _idRutina = value;
                    OnPropertyChanged();
                }
            }
        }
        public int IdEjercicioDia
        {
            get => _idEjercicioDia;
            set
            {
                if (_idEjercicioDia != value)
                {
                    _idEjercicioDia = value;
                    OnPropertyChanged();
                }
            }
        }
        public RutinaModel? Rutina
        {
            get => _rutina;
            set
            {
                if (_rutina != value)
                {
                    _rutina = value;
                    OnPropertyChanged();
                }
            }
        }
        public EjercicioDiaModel? EjercicioDia
        {
            get => _ejercicioDia;
            set
            {
                if (_ejercicioDia != value)
                {
                    _ejercicioDia = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<DiaModel> DiasSemana
        {
            get => _diasSemana;
            set
            {
                if (_diasSemana != value)
                {
                    _diasSemana = value;
                    OnPropertyChanged();
                }
            }
        }
        public DiaModel? DiaSeleccionado
        {
            get => _diaSeleccionado;
            set
            {
                if (_diaSeleccionado != value)
                {
                    _diaSeleccionado = value;
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
        public GrupoMuscularModel? GrupoMuscularSeleccionado
        {
            get => _grupoMuscularSeleccionado;
            set
            {
                if (_grupoMuscularSeleccionado != value)
                {
                    _grupoMuscularSeleccionado = value;
                    OnPropertyChanged();
                    CargarEjerciciosCommand.Execute(null);
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
        public EjercicioModel? EjercicioSeleccionado
        {
            get => _ejercicioSeleccionado;
            set
            {
                if (_ejercicioSeleccionado != value)
                {
                    _ejercicioSeleccionado = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Series
        {
            get => _series;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_series != nuevoValor)
                {
                    _series = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string Repeticiones
        {
            get => _repeticiones;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_repeticiones != nuevoValor)
                {
                    _repeticiones = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorEjercicioSeleccionado
        {
            get => _errorEjercicioSeleccionado;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_errorEjercicioSeleccionado != nuevoValor)
                {
                    _errorEjercicioSeleccionado = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorSeries
        {
            get => _errorSeries;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_errorSeries != nuevoValor)
                {
                    _errorSeries = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorRepeticiones
        {
            get => _errorRepeticiones;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_errorRepeticiones != nuevoValor)
                {
                    _errorRepeticiones = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public bool HayErrorEnEjercicioSeleccionado
        {
            get => _hayErrorEnEjercicioSeleccionado;
            set
            {
                if (_hayErrorEnEjercicioSeleccionado != value)
                {
                    _hayErrorEnEjercicioSeleccionado = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool HayErrorEnSeries
        {
            get => _hayErrorEnSeries;
            set
            {
                if (_hayErrorEnSeries != value)
                {
                    _hayErrorEnSeries = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool HayErrorEnRepeticiones
        {
            get => _hayErrorEnRepeticiones;
            set
            {
                if (_hayErrorEnRepeticiones != value)
                {
                    _hayErrorEnRepeticiones = value;
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

        //Comandos
        public ICommand CargarEjerciciosCommand { get; }
        public ICommand GuardarEjercicioCommand { get; }
        public ICommand EliminarEjercicioCommand { get; }

        //Constructor
        public AgregarEditarEjercicioRutinaViewModel(IValidacionService validacionService, IDisplayAlertService displayAlertService, IEjercicioRepositorio ejercicioRepositorio, IRutinaRepositorio rutinaRepositorio, IDiaRepositorio diaRepositorio, IEjercicioDiaRepositorio ejercicioDiaRepositorio, IGrupoMuscularRepositorio grupoMuscularRepositorio)
        {
            //Inicializan servicios con DI
            _validacionService = validacionService;
            _ejercicioRepositorio = ejercicioRepositorio;
            _rutinaRepositorio = rutinaRepositorio;
            _diaRepositorio = diaRepositorio;
            _ejercicioDiaRepositorio = ejercicioDiaRepositorio;
            _grupoMuscularRepositorio = grupoMuscularRepositorio;
            _displayAlertService = displayAlertService;

            //Inicializan comandos
            CargarEjerciciosCommand = new Command(async () => await CargarEjerciciosAsync());
            GuardarEjercicioCommand = new Command(async () => await GuardarEjercicioAsync());
            EliminarEjercicioCommand = new Command(async () => await EliminarEjercicioAsync());

        }

        // Task asincrona para cargar los datos de la rutina 
        public async Task InicializarAsync()
        {
            try
            {
                // Muestro el spinner de carga
                IsBusy = true;
                // Obtengo la rutina
                Rutina = await _rutinaRepositorio.ObtenerRutinaPorIdAsync(IdRutina);
                // Cargo los dias de la semana asociados a la rutina
                var dias = await _diaRepositorio.ObtenerDiasPorRutinaAsync(IdRutina);
                DiasSemana = new ObservableCollection<DiaModel>(dias);

                // Si la accion es agregar
                if (AccionTitulo == "Agregar")
                {
                    // Inicializa el dia seleccionado en lunes (Indice 0)
                    DiaSeleccionado = DiasSemana[0];
                    await ObtenerGruposMuscularesAsync();
                }
                // Si la accion es editar
                else if (AccionTitulo == "Editar")
                {
                    // Obtengo el EjercicioDia y Marco como dia Seleccionado su IdDia que tenga 
                    EjercicioDia = await _ejercicioDiaRepositorio.ObtenerEjercicioDiaPorIdAsync(IdEjercicioDia);
                    DiaSeleccionado = DiasSemana.FirstOrDefault(d => d.Id == EjercicioDia.IdDia);
                    await ObtenerGruposMuscularesAsync();
                    await CargarEjerciciosAsync();
                    // Obtener el ejercicio para conocer su grupo muscular
                    EjercicioSeleccionado = Ejercicios.FirstOrDefault(e => e.Id == EjercicioDia.IdEjercicio);

                    // Cargo las series y repeticiones del ejercicio
                    Series = EjercicioDia.Series.ToString();
                    Repeticiones = EjercicioDia.Repeticiones.ToString();
                }
            }
            catch (Exception)
            {
                await _displayAlertService.MostrarAlertAsync("Error", "No se pudieron cargar los datos. Intentá mas tarde.", "OK");
            }
            finally
            {
                // Oculto el spinner
                IsBusy = false;
            }
        }

        // Task asincrona para guardar el ejercicio en la rutina
        private async Task GuardarEjercicioAsync()
        {
            try
            {
                // Muestro el spinner de carga
                IsBusy = true;

                // Valido campos
                ErrorEjercicioSeleccionado = _validacionService.ValidarCampoVacio(EjercicioSeleccionado!.Nombre);
                ErrorRepeticiones = _validacionService.ValidarRepeticiones(Repeticiones);
                ErrorSeries = _validacionService.ValidarSeries(Series);
                HayErrorEnEjercicioSeleccionado = !string.IsNullOrEmpty(ErrorEjercicioSeleccionado);
                HayErrorEnRepeticiones = !string.IsNullOrEmpty(ErrorRepeticiones);
                HayErrorEnSeries = !string.IsNullOrEmpty(ErrorSeries);

                // Si hay error en un campo salgo de la funcion
                if (HayErrorEnEjercicioSeleccionado || HayErrorEnRepeticiones || HayErrorEnSeries)
                {
                    return;
                }

                // Parseo los valores de series y repeticiones
                if(!int.TryParse(Series, out var seriesInt) || !int.TryParse(Repeticiones, out var repeticionesInt))
                {
                    await _displayAlertService.MostrarAlertAsync("Error", "No se pudo guardar el ejercicio por errores en campo Series o Repeticiones. Intentá mas tarde.", "OK");
                    return;
                }

                // Filtro la accion a realizar segun la AccionTitulo que se paso por parametro de Shell
                // Si es agregar, lo agrego en la DB
                if (AccionTitulo == "Agregar")
                {
                    if (await _ejercicioDiaRepositorio.ExisteEjercicioDiaEnDiaAsync(DiaSeleccionado!.Id, EjercicioSeleccionado.Id))
                    {
                        ErrorEjercicioSeleccionado = $"Ya existe este ejercicio en el dia {DiaSeleccionado.Nombre}";
                        HayErrorEnEjercicioSeleccionado = !string.IsNullOrEmpty(ErrorEjercicioSeleccionado);
                        return;
                    }
                    await _ejercicioDiaRepositorio.AgregarEjercicioADiaAsync(DiaSeleccionado.Id, EjercicioSeleccionado.Id, seriesInt, repeticionesInt);
                }
                // Si es editar, lo actualizo en la DB
                else if (AccionTitulo == "Editar")
                {
                    await _ejercicioDiaRepositorio.ActualizarEjercicioDiaAsync(EjercicioDia!.Id, DiaSeleccionado!.Id, seriesInt, repeticionesInt);
                }

                //Envia mensaje de actualizacion para que se actualicen los datos en otros viewModels
                WeakReferenceMessenger.Default.Send(new EjercicioDiaMessages.EjercicioDiaGuardadoMessage($"Se guardó con éxito el ejercicio en el día {DiaSeleccionado!.Nombre}."));
                await Task.Delay(500);

                // Navego para atras al terminar la accion
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception)
            {
                await _displayAlertService.MostrarAlertAsync("Error", "No se pudo guardar el ejercicio. Intentá mas tarde.", "OK");
            }
            finally
            {
                // Oculto el spinner
                IsBusy = false;
            }
        }

        // Task asincrona para Eliminar el ejercicio de la rutina
        private async Task EliminarEjercicioAsync()
        {
            try
            {
                // Muestro modal de confirmacion y guardo su valor booleano
                bool confirmar = await _displayAlertService.MostrarAlertConConfirmacionAsync("Confirmar", "¿Deseas eliminar este ejercicio?", "Sí", "No");

                // Si no confirma (No), salgo de la funcion
                if (!confirmar)
                {
                    return;
                }
                // Si confirma (Si), elimino el ejercicio
                // Muestro el spinner de carga
                IsBusy = true;

                // Elimino el ejercicioDia de la DB
                await _ejercicioDiaRepositorio.EliminarEjercicioDiaAsync(EjercicioDia!.Id);

                //Envia mensaje de actualizacion para que se actualicen los datos en otros viewModels
                WeakReferenceMessenger.Default.Send(new EjercicioDiaMessages.EjercicioDiaEliminadoMessage($"Se eliminó con éxito el ejercicio en el día {DiaSeleccionado!.Nombre}."));
                await Task.Delay(500);

                // Navego para atras al terminar la accion
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception)
            {
                await _displayAlertService.MostrarAlertAsync("Error", "No se pudo eliminar el ejercicio. Intentá mas tarde.", "OK");
            }
            finally
            {
                // Oculto el spinner
                IsBusy = false;
            }
        }

        // Task asincrona para obtener los grupos musculares
        private async Task ObtenerGruposMuscularesAsync()
        {
            // Obtengo los grupos musculares
            var listaGruposMusculares = await _grupoMuscularRepositorio.ObtenerTodoGruposMuscularesAsync();
            // Lo asigno a la propiedad
            GruposMusculares = new ObservableCollection<GrupoMuscularModel>(listaGruposMusculares);
            // Agrego "Todos" con Id 0 en grupos musculares
            GruposMusculares.Insert(0, new GrupoMuscularModel { Id = 0, Nombre = "Todos" });

            // Filtro que grupo muscular seleccionar por accion(Agregar o Editar)
            if (AccionTitulo == "Agregar")
            {
                // Inicializo por defecto en Todos
                GrupoMuscularSeleccionado = GruposMusculares[0];
            }
            else
            {
                // Obtengo su grupo muscular y lo asigno a la propiedad
                var ejercicio = await _ejercicioRepositorio.ObtenerEjercicioPorIdAsync(EjercicioDia!.IdEjercicio);
                GrupoMuscularSeleccionado = GruposMusculares.FirstOrDefault(g => g.Id == ejercicio.IdGrupoMuscular);
            }
        }

        //Task asincrona para cargar los ejercicios 
        private async Task CargarEjerciciosAsync()
        {
            try
            {
                // Muestro el spinner de carga
                IsBusy = true;
                // Inicializo la lista de ejercicios
                IReadOnlyList<EjercicioModel> listaEjercicios;
                // Filtro la lista de ejercicios si es Todos o es un grupo muscular especifico
                if (GrupoMuscularSeleccionado!.Id == 0)
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
            catch (Exception)
            {
                await _displayAlertService.MostrarAlertAsync("Error", "No se pudieron cargar los ejercicios. Intentá mas tarde.", "OK");
            }
            finally
            {
                // Oculto el spinner
                IsBusy = false;
            }
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
