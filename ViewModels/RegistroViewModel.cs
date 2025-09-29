using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainiumNeon.ViewModels
{
    public class RegistroViewModel
    {
        private readonly INavigation _navigation;
        private string _nombreCompleto;
        private string _email;
        private string _contrasenia;
        private string _confirmarContrasenia;

        // Comandos
        public Command RegistrarseCommand { get; }
        public Command IniciarSesionCommand { get; }

        public RegistroViewModel(INavigation navigation)
        {
            _navigation = navigation;

            RegistrarseCommand = new Command(async () => await Registrarse());
            IniciarSesionCommand = new Command(async () => await _navigation.PopAsync());
        }

        private async Task Registrarse()
        {
            //logica de capturar, validar y mostrar errores en la UI.
            //Registro en DB

            //Envia mensaje de registro exitoso, que se va a suscrbir desde MainPage
            MessagingCenter.Send(this, "RegistroExitoso");

            //Navega a inicio de sesion para que se logee en la app.
            await _navigation.PopAsync();
        }
    }
}
