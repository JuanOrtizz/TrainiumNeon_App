using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrainiumNeon.Services;
using TrainiumNeon.Views;

namespace TrainiumNeon.ViewModels
{
    public class AgregarEditarRutinaViewModel: INotifyPropertyChanged
    {
        // Servicio
        private readonly IValidacionService _validacionService;
        // Propiedades privadas
        private string _nombreRutina;
        private string _errorNombreRutina;
        private bool _hayErrorEnNombreRutina;
        // Propiedades publicas
        public string NombreRutina
        {
            get => _nombreRutina;
            set
            {
                if(_nombreRutina != value)
                {
                    _nombreRutina = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ErrorNombreRutina
        {
            get => _errorNombreRutina;
            set
            {
                if(_errorNombreRutina != value)
                {
                    _errorNombreRutina = value;
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

        //Comandos
        public Command NavegarAgregarEjercicioCommand { get; }
        public Command GuardarRutinaCommand { get; }

        // Constructor
        public AgregarEditarRutinaViewModel(IValidacionService validacionService)
        {
            // Inicializa el servicio por DI
            _validacionService = validacionService;
            // Inicializan los comandos
            NavegarAgregarEjercicioCommand = new Command(async () => await NavegarAgregarEjercicioAsync());
            GuardarRutinaCommand = new Command(GuardarRutina);
        }

        // Task privada para navegar a agregar ejercicio
        private async Task NavegarAgregarEjercicioAsync()
        {
            await Shell.Current.GoToAsync(nameof(AgregarEjercicioRutina));
        }

        // Metodo privado para guardar rutina
        private void GuardarRutina()
        {
            ErrorNombreRutina = _validacionService.ValidarNombreRutina(NombreRutina);
            HayErrorEnNombreRutina = !string.IsNullOrEmpty(ErrorNombreRutina);
            if (HayErrorEnNombreRutina)
            {
                return;
            }
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
