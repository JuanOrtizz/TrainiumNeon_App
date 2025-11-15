using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrainiumNeon.Models;
using TrainiumNeon.Services;

namespace TrainiumNeon.ViewModels
{
    public class AgregarEjercicioRutinaViewModel: INotifyPropertyChanged
    {
        //Servicios 
        private readonly IApiEjerciciosService _apiEjerciciosService;
        private readonly IValidacionService _validacionService;
        // Propiedades privadas
        private string _diaSeleccionado;
        private ObservableCollection<string> _gruposMusculares;
        private string _grupoMuscularSeleccionado;
        private ObservableCollection<EjercicioModel> _ejercicios;
        private EjercicioModel _ejercicioSeleccionado;
        private string _series;
        private string _repeticiones;
        private string _errorEjercicioSeleccionado;
        private string _errorSeries;
        private string _errorRepeticiones;
        private bool _hayErrorEnEjercicioSeleccioando;
        private bool _hayErrorEnSeries;
        private bool _hayErrorEnRepeticiones;

        //Propiedades publicas
        public string DiaSeleccionado
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
        public ObservableCollection<string> GruposMusculares
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
        public string GrupoMuscularSeleccionado
        {
            get => _grupoMuscularSeleccionado;
            set
            {
                if (_grupoMuscularSeleccionado != value)
                {
                    _grupoMuscularSeleccionado = value;
                    _ = CargarEjerciciosAsync();
                    OnPropertyChanged();
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
        public EjercicioModel EjercicioSeleccionado
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
                if (_series != value)
                {
                    _series = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Repeticiones
        {
            get => _repeticiones;
            set
            {
                if (_repeticiones != value)
                {
                    _repeticiones = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorEjercicioSeleccionado
        {
            get => _errorEjercicioSeleccionado;
            set
            {
                if (_errorEjercicioSeleccionado != value)
                {
                    _errorEjercicioSeleccionado = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorSeries
        {
            get => _errorSeries;
            set
            {
                if (_errorSeries != value)
                {
                    _errorSeries = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorRepeticiones
        {
            get => _errorRepeticiones;
            set
            {
                if (_errorRepeticiones != value)
                {
                    _errorRepeticiones = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool HayErrorEnEjercicioSeleccionado
        {
            get => _hayErrorEnEjercicioSeleccioando;
            set
            {
                if (_hayErrorEnEjercicioSeleccioando != value)
                {
                    _hayErrorEnEjercicioSeleccioando = value;
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

        //Comandos
        public Command GuardarEjercicioCommand { get; }

        //Constructor
        public AgregarEjercicioRutinaViewModel(IApiEjerciciosService apiEjerciciosService, IValidacionService validacionService)
        {
            //Inicializan servicios con DI
            _apiEjerciciosService = apiEjerciciosService;
            _validacionService = validacionService;
            //Inicializan comandos
            GuardarEjercicioCommand = new Command(GuardarEjercicio);
            //Inicializan grupos musculares
            GruposMusculares = ObtenerGruposMusculares();
            GrupoMuscularSeleccionado = GruposMusculares[0];
            //Se cargan por primera vez los ejercicios
            _ = CargarEjerciciosAsync();
        }

        // Metodo privado para guardar el ejercicio en la rutina
        private void GuardarEjercicio()
        {
            ErrorEjercicioSeleccionado= _validacionService.ValidarEjercicioSeleccionado(EjercicioSeleccionado?.Nombre);
            ErrorRepeticiones = _validacionService.ValidarRepeticiones(Repeticiones);
            ErrorSeries = _validacionService.ValidarSeries(Series);
            HayErrorEnEjercicioSeleccionado = !string.IsNullOrEmpty(ErrorEjercicioSeleccionado);
            HayErrorEnRepeticiones = !string.IsNullOrEmpty(ErrorRepeticiones);
            HayErrorEnSeries = !string.IsNullOrEmpty(ErrorSeries);
            if(HayErrorEnEjercicioSeleccionado || HayErrorEnRepeticiones || HayErrorEnSeries)
            {
                return;
            }
        }

        //Task privado para cargar los ejercicios desde la API
        private async Task CargarEjerciciosAsync()
        {
            try
            {
                List<EjercicioModel> listaEjerciciosApi;

                if (GrupoMuscularSeleccionado == "Todos")
                {
                    listaEjerciciosApi = (List<EjercicioModel>)await _apiEjerciciosService.GetEjerciciosAsync();
                }
                else
                {
                    listaEjerciciosApi = (List<EjercicioModel>)await _apiEjerciciosService.GetEjerciciosByGrupoMuscularAsync(GrupoMuscularSeleccionado);
                }

                Ejercicios = new ObservableCollection<EjercicioModel>(listaEjerciciosApi);

            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error al usuario
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Metodo privado para obtener los grupos musculares desde el enum
        private ObservableCollection<string> ObtenerGruposMusculares()
        {
            ObservableCollection<string> coleccionGM = new ObservableCollection<string>();

            foreach (var gm in Enum.GetValues(typeof(GrupoMuscularEnum)).Cast<GrupoMuscularEnum>())
            {
                coleccionGM.Add(gm.ToString());
            }
            return coleccionGM;
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
