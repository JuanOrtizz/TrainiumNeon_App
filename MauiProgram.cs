using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using TrainiumNeon.Data;
using TrainiumNeon.Data.Repositories;
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
                .UseLocalNotification()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Servicios
            builder.Services.AddHttpClient<IApiEjerciciosService, ApiEjerciciosService>();
            builder.Services.AddSingleton<IValidacionService, ValidacionService>();
            builder.Services.AddSingleton<IEstadoContraseniaService, EstadoContraseniaService>();
            builder.Services.AddSingleton<IEjerciciosSyncService, EjerciciosSyncService>();
            builder.Services.AddSingleton<ISesionService, SesionService>();
            builder.Services.AddSingleton<IPermisosService, PermisosService>();
            builder.Services.AddSingleton<INotificacionService, NotificacionService>();
            builder.Services.AddSingleton<IDisplayAlertService, DisplayAlertService>();

            // Base de datos y repositorios
            builder.Services.AddSingleton<DatabaseService>(sp =>
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "trainiumneon.db3");
                return new DatabaseService(dbPath);
            });

            builder.Services.AddSingleton<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddSingleton<IRutinaRepositorio, RutinaRepositorio>();
            builder.Services.AddSingleton<IEjercicioRepositorio, EjercicioRepositorio>();
            builder.Services.AddSingleton<IDiaRepositorio, DiaRepositorio>();
            builder.Services.AddSingleton<IEjercicioDiaRepositorio, EjercicioDiaRepositorio>();
            builder.Services.AddSingleton<IGrupoMuscularRepositorio, GrupoMuscularRepositorio>();
            builder.Services.AddSingleton<IEstadisticasRepositorio, EstadisticasRepositorio>();

            // ViewModels
            builder.Services.AddTransient<DetalleEjercicioViewModel>();
            builder.Services.AddTransient<AgregarEditarEjercicioRutinaViewModel>();
            builder.Services.AddTransient<AgregarEditarRutinaViewModel>();
            builder.Services.AddTransient<EstadisticasViewModel>();
            builder.Services.AddTransient<MenuPrincipalViewModel>();
            builder.Services.AddTransient<PerfilViewModel>();
            builder.Services.AddTransient<RegistroViewModel>();
            builder.Services.AddTransient<RutinasViewModel>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<GimnasiosViewModel>();

            // Views
            builder.Services.AddTransient<DetalleEjercicio>();
            builder.Services.AddTransient<AgregarEditarEjercicioRutina>();
            builder.Services.AddTransient<AgregarEditarRutina>();
            builder.Services.AddTransient<Estadisticas>();
            builder.Services.AddTransient<MenuPrincipal>();
            builder.Services.AddTransient<Perfil>();
            builder.Services.AddTransient<Registro>();
            builder.Services.AddTransient<Rutinas>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<Gimnasios>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Construyo la app e inicio la DB y registros de datos iniciales.
            var app = builder.Build();

            // Inicialización async de la base de datos
            _ = InicializarAsync(app);

            //Patron observer para que DiaRepositorio se suscriba a RutinaRepositorio
            // capturo los repos
            var rutinaRepo = app.Services.GetRequiredService<IRutinaRepositorio>();
            var diaRepo = app.Services.GetRequiredService<IDiaRepositorio>();

            //Suscribo el metodo CrearDiasAsync (diaRepo) al evento CreandoRutina (rutinaRepo)
            rutinaRepo.CreandoRutina += (idRutina) =>
            {
               return diaRepo.CrearDiasAsync(idRutina);
            };

            return app;
        }

        // Task asincrona para inicializar la base de datos y registrar datos iniciales
        private static async Task InicializarAsync(MauiApp app)
        {
            try
            {
                using var scope = app.Services.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<DatabaseService>();
                await db.InitializeAsync();

                var grupos = scope.ServiceProvider.GetRequiredService<IGrupoMuscularRepositorio>();
                await grupos.RegistrarGruposMuscularesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inicializando la BD: {ex.Message}");
            }
        }
    }
}
