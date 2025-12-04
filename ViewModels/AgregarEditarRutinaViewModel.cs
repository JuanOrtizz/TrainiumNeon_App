using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TrainiumNeon.Data.Repositories;
using TrainiumNeon.Messages;
using TrainiumNeon.Models;
using TrainiumNeon.Services;
using TrainiumNeon.Views;

namespace TrainiumNeon.ViewModels
{
    [QueryProperty(nameof(AccionTitulo), "accion")]
    [QueryProperty(nameof(IdRutina), "idRutina")]
    public class AgregarEditarRutinaViewModel: INotifyPropertyChanged
    {
        // Servicios y repositorios
        private readonly IValidacionService _validacionService;
        private readonly ISesionService _sesionService;
        private readonly IRutinaRepositorio _rutinaRepositorio;
        private readonly IDiaRepositorio _diaRepositorio;
        private readonly IEjercicioRepositorio _ejercicioRepositorio;
        private readonly IEjercicioDiaRepositorio _ejercicioDiaRepositorio;

        // Propiedad de lectura
        public bool EsEditar => AccionTitulo == "Editar";

        // Propiedades privadas
        private int _idUsuarioActivo;
        private string _accionTitulo = string.Empty;
        private int _idRutina;
        private RutinaModel _rutina;
        private string _nombreRutina = string.Empty;
        private string _errorNombreRutina = string.Empty;
        private bool _hayErrorEnNombreRutina;
        private ObservableCollection<DiaModel> _diasSemana = new ObservableCollection<DiaModel>();
        private DiaModel _diaSeleccionado;
        private bool _puedeIrAnterior;
        private bool _puedeIrSiguiente;
        private ObservableCollection<EjercicioDiaModel> _ejerciciosDiaSeleccionado = new ObservableCollection<EjercicioDiaModel>();
        private bool _isBusy;
        private bool _puedeActualizar = false;
        private bool _alertMostrado = false;

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
        public RutinaModel Rutina
        {
            get => _rutina;
            set 
            {
                if(_rutina != value)
                {
                    _rutina = value;
                    OnPropertyChanged();
                }
            }
        }
        public string NombreRutina
        {
            get => _nombreRutina;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_nombreRutina != nuevoValor)
                {
                    _nombreRutina = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorNombreRutina
        {
            get => _errorNombreRutina;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_errorNombreRutina != nuevoValor)
                {
                    _errorNombreRutina = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public bool HayErrorEnNombreRutina
        {
            get => _hayErrorEnNombreRutina;
            set
            {
                if(_hayErrorEnNombreRutina != value)
                {
                    _hayErrorEnNombreRutina = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<DiaModel> DiasSemana
        {
            get => _diasSemana;
            set
            {
                if(_diasSemana != value)
                {
                    _diasSemana = value;
                    OnPropertyChanged();
                }
            }
        }
        public DiaModel DiaSeleccionado
        {
            get => _diaSeleccionado;
            set
            {
                if(_diaSeleccionado != value)
                {
                    _diaSeleccionado = value;
                    _ = CargarEjerciciosDiaSeleccionadoAsync();
                    OnPropertyChanged();
                    ActualizarEstadosNavegacionDias();
                }
            }
        }
        public bool PuedeIrAnterior
        {
            get => _puedeIrAnterior;
            set
            {
                if (_puedeIrAnterior != value)
                {
                    _puedeIrAnterior = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool PuedeIrSiguiente
        {
            get => _puedeIrSiguiente;
            set
            {
                if (_puedeIrSiguiente != value)
                {
                    _puedeIrSiguiente = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<EjercicioDiaModel> EjerciciosDiaSeleccionado
        {
            get => _ejerciciosDiaSeleccionado;
            set
            {
                if (_ejerciciosDiaSeleccionado != value)
                {
                    _ejerciciosDiaSeleccionado = value;
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
                if (_puedeActualizar != value)
                {
                    _puedeActualizar = value;
                    OnPropertyChanged();
                }
            }
        }

        //Comandos
        public ICommand NavegarAgregarEjercicioCommand { get; }
        public ICommand NavegarEditarEjercicioCommand { get; }
        public ICommand GuardarRutinaCommand { get; }
        public ICommand EliminarRutinaCommand { get; }
        public ICommand NavegarDiaAnteriorCommand { get; }
        public ICommand NavegarDiaSiguienteCommand { get; }

        // Constructor
        public AgregarEditarRutinaViewModel(IValidacionService validacionService, ISesionService sesionService, IRutinaRepositorio rutinaRepositorio, IDiaRepositorio diaRepositorio, IEjercicioRepositorio ejercicioRepositorio, IEjercicioDiaRepositorio ejercicioDiaRepositorio)
        {
            // Inicializan servicios y repositorios por DI
            _validacionService = validacionService;
            _sesionService = sesionService;
            _rutinaRepositorio = rutinaRepositorio;
            _diaRepositorio = diaRepositorio;
            _ejercicioRepositorio = ejercicioRepositorio;
            _ejercicioDiaRepositorio = ejercicioDiaRepositorio;

            // Inicializan los comandos
            NavegarAgregarEjercicioCommand = new Command(async () => await NavegarAgregarEjercicioAsync());
            NavegarEditarEjercicioCommand = new Command<int>(async (idEjercicioDia) => await NavegarEditarEjercicioAsync(idEjercicioDia));
            GuardarRutinaCommand = new Command(async () => await GuardarRutinaAsync());
            EliminarRutinaCommand = new Command(async () => await EliminarRutinaAsync());
            NavegarDiaAnteriorCommand = new Command(NavegarDiaAnterior);
            NavegarDiaSiguienteCommand = new Command(NavegarDiaSiguiente);
            
        }

        //Task asincrona para inicializar desde Code-behind con asincronia y evitar referencias null de Query-Property
        public async Task InicializarAsync()
        {
            // Capturo el Id del usuario activo
            IdUsuarioActivo = _sesionService.ObtenerSesion();
            // Cargo la rutina
            await CargarRutinaAsync();
        }

        // Task asincrona para guardar la rutina
        private async Task GuardarRutinaAsync()
        {
            // Muestro el spinner 
            IsBusy = true;
            // Valida campos
            ErrorNombreRutina = _validacionService.ValidarNombreRutina(NombreRutina);
            HayErrorEnNombreRutina = !string.IsNullOrEmpty(ErrorNombreRutina);
            // Si hay errores salgo de la funcion y oculto el spinner
            if (HayErrorEnNombreRutina)
            {
                IsBusy = false;
                return;
            }

            if(NombreRutina != Rutina.Nombre && await _rutinaRepositorio.ExisteRutinaConNombreAsync(IdUsuarioActivo, NombreRutina))
            {
                IsBusy = false;
                ErrorNombreRutina = "Ya existe una rutina con este nombre.";
                HayErrorEnNombreRutina = !string.IsNullOrEmpty(ErrorNombreRutina);
                return;
            }

            // Actualizo la rutina en la DB
            await _rutinaRepositorio.ActualizarRutinaVaciaAsync(Rutina.Id, IdUsuarioActivo, NombreRutina);

            //Envia mensaje de actualizacion para que se actualicen los datos en otros viewModels
            WeakReferenceMessenger.Default.Send(new RutinaMessages.RutinaGuardadaMessage("Se Guardó con éxito la rutina."));
            await Task.Delay(500);

            // Actualiza PuedeActualizar y muestra toast de exito
            PuedeActualizar = false;
            // Oculto el spinner
            IsBusy = false;

            // Vuelvo para atras al guardar
            await Shell.Current.GoToAsync("..");
        }

        // Task asincrona para eliminar la rutina
        private async Task EliminarRutinaAsync()
        {
            if (await _rutinaRepositorio.EsRutinaSeleccionada(IdUsuarioActivo, IdRutina))
            {
                // Si no la elimino muestro mensaje de error
                await Application.Current.MainPage.DisplayAlert("Error", "No se puede eliminar una rutina seleccionada, cambia de rutina seleccionada para poder eliminarla.", "OK");
                return;
            }
            // Elimino la rutina
            bool eliminado = await _rutinaRepositorio.EliminarRutinaAsync(IdRutina);
            // Si la elimino
            if (eliminado)
            {
                // Vuelvo a la pagina anterior
                //Envia mensaje de actualizacion para que se actualicen los datos en otros viewModels
                WeakReferenceMessenger.Default.Send(new RutinaMessages.RutinaEliminadaMessage("Se eliminó con éxito la rutina."));

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                // Si no la elimino muestro mensaje de error
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar la rutina.", "OK");
            }
        }

        // Task asincrona para cargar la accion de la rutina
        public async Task CargarRutinaAsync()
        {
            // Muestro el spinner de carga
            IsBusy = true;
            // Si es Agregar
            if (AccionTitulo == "Agregar")
            {
                
                // Verifica si hay una rutina no guardada (Borrador) del usuario
                if (await _rutinaRepositorio.ExisteRutinaEnBorradorAsync(IdUsuarioActivo))
                {
                    // Carga la rutina en borrador
                    Rutina = await _rutinaRepositorio.ObtenerRutinaEnBorradorAsync(IdUsuarioActivo);
                    // Capturo el Id de la rutina
                    IdRutina = Rutina.Id;
                    await ObtenerDiasRutina();
                    // Oculto el spinner
                    IsBusy = false;
                    // Muestro un alert para indicar que se cargo el borrador no guardado
                    // Solo mostrar el alert una vez
                    if (!_alertMostrado)
                    {
                        _alertMostrado = true;
                        await Application.Current.MainPage.DisplayAlert("Borrador Rutina", "Se cargó con exito el borrador de la rutina que no guardaste.", "OK");
                    }
                    
                    // Salgo de la funcion
                    return;
                }
                // Creo una rutina temporal (Si el usuario abandona la almacena como borrador)
                else if (IdUsuarioActivo > 0)
                {
                    // Creo la rutina (Borrador) y obtengo su Id 
                    Rutina = await _rutinaRepositorio.CrearRutinaVaciaAsync(IdUsuarioActivo);
                    IdRutina = Rutina.Id;
                    await ObtenerDiasRutina();
                    // Oculto el spinner
                    IsBusy = false;
                }
            }
            // Si es editar
            else
            {
                // Obtengo la rutina y su nombre
                Rutina = await _rutinaRepositorio.ObtenerRutinaPorIdAsync(IdRutina);
                NombreRutina = Rutina.Nombre;
                await ObtenerDiasRutina();
                // Oculto el spinner
                IsBusy = false;
            }
        }

        // Task asincrona para cargar los ejercicios del dia seleccionado
        private async Task CargarEjerciciosDiaSeleccionadoAsync()
        {
            // Inicializo la lista de ejercicios
            IReadOnlyList<EjercicioDiaModel> ejerciciosDia;
            // Capturo los ejercicios del dia y recorro la lista para agregarle el ejercicio y poder acceder a su nombre
            ejerciciosDia = await _ejercicioDiaRepositorio.ObtenerEjerciciosPorDiaAsync(DiaSeleccionado.Id);
            foreach(var ed in ejerciciosDia)
            {
                ed.Ejercicio = await _ejercicioRepositorio.ObtenerEjercicioPorIdAsync(ed.IdEjercicio);
            }
            EjerciciosDiaSeleccionado = new ObservableCollection<EjercicioDiaModel>(ejerciciosDia);
        }

        // Task asincrona para obtener los dias de la rutina y asignarlos a las propiedades DiasSemana y DiaSeleccionado
        private async Task ObtenerDiasRutina()
        {
            // Cargo los dias de la semana asociados a la rutina
            var dias = await _diaRepositorio.ObtenerDiasPorRutinaAsync(IdRutina);
            DiasSemana = new ObservableCollection<DiaModel>(dias);
            // Inicializa los dias de la rutina en lunes (Indice 0)
            DiaSeleccionado = DiasSemana[0];
        }

        // Metodo para navegar al dia anterior de la semana en la rutina
        private void NavegarDiaAnterior()
        {
            // Capturo el indice del dia seleccionado
            var indiceDiaSeleccionado = DiasSemana.IndexOf(DiaSeleccionado);
            if (indiceDiaSeleccionado > 0)
            {
                // Cambio el dia seleccionado al dia anterior 
                DiaSeleccionado = DiasSemana[indiceDiaSeleccionado - 1];
            }
        }

        // Metodo para navegar al dia siguiente de la semana en la rutina
        private void NavegarDiaSiguiente()
        {
            // Capturo el indice del dia seleccionado
            var indiceDiaSeleccionado = DiasSemana.IndexOf(DiaSeleccionado);
            if (indiceDiaSeleccionado < DiasSemana.Count - 1)
            {
                // Cambio el dia seleccionado al dia siguiente 
                DiaSeleccionado = DiasSemana[indiceDiaSeleccionado + 1];
            }
        }

        // Metodo para actualizar el estado de los botones de navegacion de los dias
        private void ActualizarEstadosNavegacionDias()
        {
            // Si los dias no son null, 0 o no hay dia seleccionado, retorno
            if (DiasSemana == null || DiasSemana.Count == 0 || DiaSeleccionado == null)
            {
                PuedeIrAnterior = false;
                PuedeIrSiguiente = false;
                return;
            }

            // Capturo el indice del dia seleccionado
            var indice = DiasSemana.IndexOf(DiaSeleccionado);
            // Devuelvo si puede volver o avanzar en los dias de la rutina
            PuedeIrAnterior = indice > 0;
            PuedeIrSiguiente = indice < DiasSemana.Count - 1;
        }

        // Task asincrona para navegar a agregar ejercicio
        private async Task NavegarAgregarEjercicioAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(AgregarEditarEjercicioRutina)}?accion=Agregar&idRutina={IdRutina}");
        }

        // Task asincrona para navegar a editar ejercicio
        private async Task NavegarEditarEjercicioAsync(int idEjercicioDia)
        {
            await Shell.Current.GoToAsync($"{nameof(AgregarEditarEjercicioRutina)}?accion=Editar&idRutina={IdRutina}&idEjercicioDia={idEjercicioDia}");
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
