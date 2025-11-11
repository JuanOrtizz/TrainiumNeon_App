using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainiumNeon.Views;

namespace TrainiumNeon.ViewModels
{
    public class AgregarEditarRutinaViewModel
    {

        public Command NavegarAgregarEjercicioCommand { get; }

        public AgregarEditarRutinaViewModel()
        {
            NavegarAgregarEjercicioCommand = new Command(async () => await NavegarAgregarEjercicioAsync());
        }

        private async Task NavegarAgregarEjercicioAsync()
        {
            await Shell.Current.GoToAsync(nameof(AgregarEjercicioRutina));
        }
    }
}
