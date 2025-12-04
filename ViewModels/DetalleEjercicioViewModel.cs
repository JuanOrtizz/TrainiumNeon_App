using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TrainiumNeon.Data.Repositories;
using TrainiumNeon.Models;
using TrainiumNeon.Services;

namespace TrainiumNeon.ViewModels
{
    [QueryProperty(nameof(IdEjercicio), "idEjercicio")]
    public class DetalleEjercicioViewModel: INotifyPropertyChanged
    {
        // Servicio y repositorio
        private readonly IValidacionService _validacionService;
        private readonly IEjercicioRepositorio _ejercicioRepositorio;
        private readonly IGrupoMuscularRepositorio _grupoMuscularRepositorio;
        private readonly IEstadisticasRepositorio _estadisticasRepositorio;

        // Propiedades privadas
        private int _idEjercicio;
        private EjercicioModel _ejercicio;
        private string _nombreEjercicio = string.Empty;
        private string _grupoMuscularEjercicio = string.Empty;
        private string _ImagenUrlEjercicio = string.Empty;
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
                } 
            }
        }
        public EjercicioModel Ejercicio
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
            get => _ImagenUrlEjercicio;
            set
            {
                var nuevoValor = value?.Trim() ?? string.Empty;
                if (_ImagenUrlEjercicio != nuevoValor)
                {
                    _ImagenUrlEjercicio = nuevoValor;
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
        public DetalleEjercicioViewModel(IValidacionService validacionService, IEjercicioRepositorio ejercicioRepositorio, IGrupoMuscularRepositorio grupoMuscularRepositorio, IEstadisticasRepositorio estadisticasRepositorio)
        {
            // Inicializa servicio y repositorio por DI
            _validacionService = validacionService;
            _ejercicioRepositorio = ejercicioRepositorio;
            _grupoMuscularRepositorio = grupoMuscularRepositorio;
            _estadisticasRepositorio = estadisticasRepositorio;
            // Inicializa comando
            GuardarPersonalRecordCommand = new Command(async () => await GuardarPersonalRecordAsync());
        }

        // Task asincrona para cargar detalles del ejercicio
        public async Task InicializarAsync()
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
            // Oculto spinner de carga
            IsBusy = false;
        }

        // Task asincrona para guardar personal record
        private async Task GuardarPersonalRecordAsync()
        {
            // Muestro el spinner de carga
            IsBusy = true;
            // Valido campo
            ErrorPersonalRecord = _validacionService.ValidarPR(PersonalRecord);
            HayErrorEnPersonalRecord = !string.IsNullOrEmpty(ErrorPersonalRecord);
            if (HayErrorEnPersonalRecord)
            {
                IsBusy = false;
                return;
            }
            // Si no hay errores, guarda el PR
            var nuevoPR = int.Parse(PersonalRecord);
            await _ejercicioRepositorio.ActualizarPersonalRecordAsync(IdEjercicio, nuevoPR);
            await Task.Delay(500);
            IsBusy = false;
            var toast = Toast.Make("Actualizaste tu Personal Record", ToastDuration.Short);
            await toast.Show();
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
