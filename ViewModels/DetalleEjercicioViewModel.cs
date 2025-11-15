using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrainiumNeon.Models;
using TrainiumNeon.Services;

namespace TrainiumNeon.ViewModels
{
    [QueryProperty(nameof(IdEjercicio), "idEjercicio")]
    public class DetalleEjercicioViewModel: INotifyPropertyChanged
    {
        // Servicios
        private readonly IApiEjerciciosService _apiEjerciciosService;
        private readonly IValidacionService _validacionService;
        // Propiedades privadas
        private int idEjercicio;
        private EjercicioModel ejercicio;
        private string _personalRecord;
        private string _errorPersonalRecord;
        private bool _hayErrorEnPersonalRecord;
        // Propiedades publicas
        public int IdEjercicio
        {
            get => idEjercicio;
            set
            {
                if(idEjercicio != value)
                {
                    idEjercicio = value;
                    _ = CargarEjercicioAsync();
                } 
            }
        }
        public EjercicioModel Ejercicio
        {
            get => ejercicio;
            set
            {
                if(ejercicio != value)
                {
                    ejercicio = value;
                    OnPropertyChanged(nameof(Ejercicio));
                    OnPropertyChanged(nameof(Nombre));
                    OnPropertyChanged(nameof(GrupoMuscular));
                    OnPropertyChanged(nameof(ImagenUrl));
                }
            }
        }
        public string Nombre => Ejercicio?.Nombre ?? "";
        public string GrupoMuscular => Ejercicio?.GrupoMuscular ?? "";
        public string ImagenUrl => Ejercicio?.ImagenUrl ?? "default_ejercicio.webp";
        public string PersonalRecord
        {
            get => _personalRecord;
            set
            {
                if (_personalRecord != value)
                {
                    _personalRecord = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorPersonalRecord
        {
            get => _errorPersonalRecord;
            set
            {
                if (_errorPersonalRecord != value)
                {
                    _errorPersonalRecord = value;
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

        // Comando
        public Command GuardarPersonalRecordCommand { get; }

        // Constructor
        public DetalleEjercicioViewModel(IApiEjerciciosService apiEjerciciosService, IValidacionService validacionService)
        {
            // Inicializan servicios por DI
            _apiEjerciciosService = apiEjerciciosService;
            _validacionService = validacionService;
            // Inicializa comando
            GuardarPersonalRecordCommand = new Command(GuardarPersonalRecord);
        }

        // Task privada para cargar ejercicio (detalles) desde API
        private async Task CargarEjercicioAsync()
        {
            try
            {
                var ejercicioApi = await _apiEjerciciosService.GetEjercicioByIdAsync(idEjercicio);
                Ejercicio = ejercicioApi;
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error al usuario
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Metodo privado para guardar personal record
        private void GuardarPersonalRecord()
        {
            ErrorPersonalRecord = _validacionService.ValidarPR(PersonalRecord);
            HayErrorEnPersonalRecord = !string.IsNullOrEmpty(ErrorPersonalRecord);
            if (HayErrorEnPersonalRecord)
            {
                return;
            }
            // Logica para guardar el personal record se agrega despues
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
