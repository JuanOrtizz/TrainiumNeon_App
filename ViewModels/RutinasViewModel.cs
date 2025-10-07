using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainiumNeon.Views;

namespace TrainiumNeon.ViewModels
{
    public class RutinasViewModel
    {
        public Command AgregarRutinaCommand { get; }

        public RutinasViewModel()
        {
            AgregarRutinaCommand = new Command(async () => await NavegarAgregarRutina());
        }

        private async Task NavegarAgregarRutina()
        {
            await Shell.Current.GoToAsync(nameof(AgregarEditarRutina));
        }
    }
}
