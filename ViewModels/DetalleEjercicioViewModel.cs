using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TrainiumNeon.Data.Repositories;
using TrainiumNeon.Messages;
using TrainiumNeon.Models;
using TrainiumNeon.Services;

namespace TrainiumNeon.ViewModels
{
    [QueryProperty(nameof(IdEjercicio), "idEjercicio")]
    public class DetalleEjercicioViewModel: INotifyPropertyChanged
    {
        // Servicios y repositorios
        private readonly IValidacionService _validacionService;
        private readonly IDisplayAlertService _displayAlertService;
        private readonly IEjercicioRepositorio _ejercicioRepositorio;
        private readonly IGrupoMuscularRepositorio _grupoMuscularRepositorio;
        private readonly IEstadisticasRepositorio _estadisticasRepositorio;

        // Propiedades privadas
        private int _idEjercicio;
        private EjercicioModel? _ejercicio;
        private string _nombreEjercicio = string.Empty;
        private string _grupoMuscularEjercicio = string.Empty;
        private string _imagenUrlEjercicio = string.Empty;
        private int _aparicionesEnRutinas;
        private string _personalRecord = string.Empty;
        private string _errorPersonalRecord = string.Empty;
        private bool _hayErrorEnPersonalRecord;
        private bool _isBusy;

        // Propiedades publicas
        public int IdEjercicio
        {
            get => _idEjercicio;
            set
            {
                if(_idEjercicio != value)
                {
                    _idEjercicio = value;
                    OnPropertyChanged();
                } 
            }
        }
        public EjercicioModel? Ejercicio
        {
            get => _ejercicio;
            set
            {
                if(_ejercicio != value)
                {
                    _ejercicio = value;
                    OnPropertyChanged();
                }
            }
        }
        public string NombreEjercicio
        {
            get => _nombreEjercicio;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_nombreEjercicio != nuevoValor)
                {
                    _nombreEjercicio = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string GrupoMuscularEjercicio
        {
            get => _grupoMuscularEjercicio;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_grupoMuscularEjercicio != nuevoValor)
                {
                    _grupoMuscularEjercicio = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ImagenUrlEjercicio
        {
            get => _imagenUrlEjercicio;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_imagenUrlEjercicio != nuevoValor)
                {
                    _imagenUrlEjercicio = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public int AparicionesEnRutinas
        {
            get => _aparicionesEnRutinas;
            set
            {
                if (_aparicionesEnRutinas != value)
                {
                    _aparicionesEnRutinas = value;
                    OnPropertyChanged();
                }
            }
        }
        public string PersonalRecord
        {
            get => _personalRecord;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_personalRecord != nuevoValor)
                {
                    _personalRecord = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorPersonalRecord
        {
            get => _errorPersonalRecord;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_errorPersonalRecord != nuevoValor)
                {
                    _errorPersonalRecord = nuevoValor;
                    OnPropertyChanged();
                }
            }
        }
        public bool HayErrorEnPersonalRecord
        {
            get => _hayErrorEnPersonalRecord;
            set
            {
                if (_hayErrorEnPersonalRecord != value)
                {
                    _hayErrorEnPersonalRecord = value;
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

        // Comando
        public ICommand GuardarPersonalRecordCommand { get; }

        // Constructor
        public DetalleEjercicioViewModel(IValidacionService validacionService, IDisplayAlertService displayAlertService , IEjercicioRepositorio ejercicioRepositorio, IGrupoMuscularRepositorio grupoMuscularRepositorio, IEstadisticasRepositorio estadisticasRepositorio)
        {
            // Inicializa servicio y repositorios por DI
            _validacionService = validacionService;
            _displayAlertService = displayAlertService;
            _ejercicioRepositorio = ejercicioRepositorio;
            _grupoMuscularRepositorio = grupoMuscularRepositorio;
            _estadisticasRepositorio = estadisticasRepositorio;
            // Inicializa comando
            GuardarPersonalRecordCommand = new Command(async () => await GuardarPersonalRecordAsync());
        }

        // Task asincrona para cargar detalles del ejercicio
        public async Task InicializarAsync()
        {
            try
            {
                // Muestro spinner de carga
                IsBusy = true;
                // Capturo el ejercicio, su grupo muscular y las apariciones en rutinas de ese ejercicio
                Ejercicio = await _ejercicioRepositorio.ObtenerEjercicioPorIdAsync(IdEjercicio);
                var grupoMuscular = await _grupoMuscularRepositorio.ObtenerGrupoMuscularPorIdAsync(Ejercicio.IdGrupoMuscular);
                AparicionesEnRutinas = await _estadisticasRepositorio.ObtenerCantidadDeAparicionesEnRutinasAsync(IdEjercicio);
                //Actualizo propiedades
                NombreEjercicio = Ejercicio.Nombre;
                GrupoMuscularEjercicio = grupoMuscular.Nombre;
                ImagenUrlEjercicio = Ejercicio.ImagenUrl ?? "default_ejercicio.webp";
                PersonalRecord = Ejercicio.PersonalRecord.ToString();
            }
            catch (Exception)
            {
                await _displayAlertService.MostrarAlertAsync("Error", "No se pudieron cargar los detalles del ejercicio. Intentá mas tarde.", "OK");
            }
            finally
            {
                // Oculto spinner de carga
                IsBusy = false;
            }
        }

        // Task asincrona para guardar personal record
        private async Task GuardarPersonalRecordAsync()
        {
            try
            {
                // Muestro el spinner de carga
                IsBusy = true;
                // Valido campo
                ErrorPersonalRecord = _validacionService.ValidarPR(PersonalRecord);
                HayErrorEnPersonalRecord = !string.IsNullOrEmpty(ErrorPersonalRecord);
                if (HayErrorEnPersonalRecord)
                {
                    return;
                }
                // Si no hay errors parseo el PR y verifico si hubo cambios
                if (!int.TryParse(PersonalRecord, out var nuevoPR))
                {
                    Console.WriteLine("Error al parsear el Personal Record.");
                    return;
                }
                if (nuevoPR == Ejercicio?.PersonalRecord)
                {
                    return; // No hay cambios y salgo de la funcion para evitar llamada innecesaria a la base de datos
                }
                // Actualizo el PR en la base de datos
                await _ejercicioRepositorio.ActualizarPersonalRecordAsync(IdEjercicio, nuevoPR);

                //Envia mensaje de actualizacion para que se actualicen los datos en otros viewModels
                WeakReferenceMessenger.Default.Send(new EjercicioMessages.PRActualizadoMessage());

                await Task.Delay(500);
                // Muestro toast de exito
                var toast = Toast.Make("Actualizaste tu Personal Record", ToastDuration.Short);
                await toast.Show();
            }
            catch (Exception)
            {
                await _displayAlertService.MostrarAlertAsync("Error", "No se pudo actualizar el PR del ejercicio.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
