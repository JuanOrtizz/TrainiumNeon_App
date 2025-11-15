using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using TrainiumNeon.Services;
using TrainiumNeon.ViewModels;
using TrainiumNeon.Views;

namespace TrainiumNeon
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            //Servicios
            builder.Services.AddHttpClient<IApiEjerciciosService, ApiEjerciciosService>();
            builder.Services.AddSingleton<IValidacionService, ValidacionService>();
            builder.Services.AddSingleton<IEstadoContraseniaService, EstadoContraseniaService>();

            // ViewModels
            builder.Services.AddTransient<DetalleEjercicioViewModel>();
            builder.Services.AddTransient<AgregarEjercicioRutinaViewModel>();
            builder.Services.AddTransient<AgregarEditarRutinaViewModel>();
            builder.Services.AddTransient<EstadisticasViewModel>();
            builder.Services.AddTransient<MenuPrincipalViewModel>();
            builder.Services.AddTransient<PerfilViewModel>();
            builder.Services.AddTransient<RegistroViewModel>();
            builder.Services.AddTransient<RutinasViewModel>();
            builder.Services.AddTransient<MainPageViewModel>();

            // Views
            builder.Services.AddTransient<DetalleEjercicio>();
            builder.Services.AddTransient<AgregarEjercicioRutina>();
            builder.Services.AddTransient<AgregarEditarRutina>();
            builder.Services.AddTransient<Estadisticas>();
            builder.Services.AddTransient<MenuPrincipal>();
            builder.Services.AddTransient<Perfil>();
            builder.Services.AddTransient<Registro>();
            builder.Services.AddTransient<Rutinas>();
            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
