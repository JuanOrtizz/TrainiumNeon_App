using TrainiumNeon.Data.Repositories;

namespace TrainiumNeon.Services
{
    public class EjerciciosSyncService : IEjerciciosSyncService
    {
        // Servicio y repositorios
        private readonly IApiEjerciciosService _apiEjercicioService;
        private readonly IEjercicioRepositorio _ejercicioRepositorio;
        private readonly IGrupoMuscularRepositorio _grupoMuscularRepositorio;

        // Constructor
        public EjerciciosSyncService(IApiEjerciciosService apiEjercicioService, IEjercicioRepositorio ejercicioRepositorio, IGrupoMuscularRepositorio grupoMuscularRepositorio)
        {
            // Inicializa el servicio y los repositorios
            _apiEjercicioService = apiEjercicioService;
            _ejercicioRepositorio = ejercicioRepositorio;
            _grupoMuscularRepositorio = grupoMuscularRepositorio;
        }

        // Task asincrona para sincronizar los ejercicios de la API con la DB 
        public async Task SincronizarEjerciciosAsync()
        {

            //Verifico si los ejercicios se sincronizaron hace menos de 24 horas
            var ultimaSincronizacion = Preferences.Get("UltimaSyncEjercicios", DateTime.MinValue);
            if ((DateTime.Now - ultimaSincronizacion).TotalHours < 24)
            {
                return; // No es necesario sincronizar y salgo de la task
            }

            // Verifico conexion a internet
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert( "Sin conexión", "No se pudieron sincronizar los ejercicios. Tene en cuenta que estas viendo una version desactualizada de los ejercicios y pueden cambiar al sincronizarse.", "OK");
                return;
            }

            // Capturo los ejercicios de la API y de la DB
            var listaEjerciciosApi = await _apiEjercicioService.GetEjerciciosAsync();
            var listaEjerciciosLocal = await _ejercicioRepositorio.ObtenerTodosLosEjerciciosAsync();

            // Si la tabla (DB) esta vacia, inserto todos los ejercicios de la API
            if (listaEjerciciosLocal.Count == 0)
            {
                foreach(var e in listaEjerciciosApi)
                {
                    var grupoMuscular = await _grupoMuscularRepositorio.ObtenerGrupoMuscularPorIdAsync(e.IdGrupoMuscular);
                    await _ejercicioRepositorio.CrearEjercicioAsync(e.Nombre, e.IdGrupoMuscular, e.ImagenUrl);
                }
                // Actualizo la fecha de la ultima sincronizacion
                Preferences.Set("UltimaSyncEjercicios", DateTime.Now);
                return;
            }

            // Si hay ejercicios en la tabla, verifico si hay que insertar nuevos o actualizar existentes
            foreach (var ejercicioApi in listaEjerciciosApi)
            {
                // Verifico si el ejercicio de la API ya existe en la base de datos local
                var ejercicioLocal = listaEjerciciosLocal.FirstOrDefault(e => e.Nombre == ejercicioApi.Nombre);
                // Si no existe lo inserto
                if (ejercicioLocal == null)
                {
                    await _ejercicioRepositorio.CrearEjercicioAsync(ejercicioApi.Nombre, ejercicioApi.IdGrupoMuscular, ejercicioApi.ImagenUrl);
                }
                else// Si no verifico si hay que actualizazrlo desde la API
                {
                    if (ejercicioLocal.IdGrupoMuscular != ejercicioApi.IdGrupoMuscular || ejercicioLocal.ImagenUrl != ejercicioApi.ImagenUrl)
                    {
                        await _ejercicioRepositorio.ActualizarEjercicioAsync(ejercicioLocal.Id, ejercicioApi.IdGrupoMuscular, ejercicioApi.ImagenUrl);
                    }
                }
            }
            // Actualizo la fecha de la ultima sincronizacion
            Preferences.Set("UltimaSyncEjercicios", DateTime.Now);
        }
    }
}
