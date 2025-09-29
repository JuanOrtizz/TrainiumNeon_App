using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainiumNeon.Views;
using CommunityToolkit.Maui.Alerts;

namespace TrainiumNeon.ViewModels
{
    public class MainPageViewModel
    {

        // Variables
        private readonly INavigation _navigation;
        private string _email;
        private string _contrasenia;


        // Comandos
        public Command RegistroCommand { get; }
        public Command IniciarSesionCommand { get; }

        public MainPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            RegistroCommand = new Command(async () => await Registrarse());
            IniciarSesionCommand = new Command(async() => await IniciarSesion());

            MessagingCenter.Subscribe<RegistroViewModel>(this, "RegistroExitoso", async (sender) =>
            {
                var toast = Toast.Make("Registro exitoso. Ahora inicia sesión.", ToastDuration.Short);
                await toast.Show();
            });
        }
            

        private async Task Registrarse()
        {
            var registroPage = new Registro();

            registroPage.Opacity = 0;
            registroPage.TranslationX = 300;

            await _navigation.PushAsync(registroPage, false);

            await Task.WhenAll(
                registroPage.TranslateTo(0, 0, 300, Easing.SinOut),
                registroPage.FadeTo(1, 500, Easing.CubicInOut)
            );
        }

        private async Task IniciarSesion()
        {
            Application.Current.MainPage = new AppShell();
        }
    }
}
