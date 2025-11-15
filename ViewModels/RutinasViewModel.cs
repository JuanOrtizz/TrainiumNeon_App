using TrainiumNeon.Views;

namespace TrainiumNeon.ViewModels
{
    public class RutinasViewModel
    {
        // Comando 
        public Command AgregarRutinaCommand { get; }

        // Constructor
        public RutinasViewModel()
        {
            // Inicializa comando
            AgregarRutinaCommand = new Command(async () => await NavegarAgregarRutina());
        }

        // Task privada para navegar a la pagina de agregar rutina
        private async Task NavegarAgregarRutina()
        {
            await Shell.Current.GoToAsync(nameof(AgregarEditarRutina));
        }
    }
}
