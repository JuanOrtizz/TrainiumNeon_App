using TrainiumNeon.Data.Repositories;

namespace TrainiumNeon.Services
{
    public class EjerciciosSyncService : IEjerciciosSyncService
    {
        // Servicio y repositorios
        private readonly IApiEjerciciosService _apiEjercicioService;
        private readonly IDisplayAlertService _displayAlertService;
        private readonly IEjercicioRepositorio _ejercicioRepositorio;
        private readonly IGrupoMuscularRepositorio _grupoMuscularRepositorio;

        // Constructor
        public EjerciciosSyncService(IApiEjerciciosService apiEjercicioService, IDisplayAlertService displayAlertService ,IEjercicioRepositorio ejercicioRepositorio, IGrupoMuscularRepositorio grupoMuscularRepositorio)
        {
            // Inicializa el servicio y los repositorios
            _apiEjercicioService = apiEjercicioService;
            _displayAlertService = displayAlertService;
            _ejercicioRepositorio = ejercicioRepositorio;
            _grupoMuscularRepositorio = grupoMuscularRepositorio;
        }

        // Task asincrona para sincronizar los ejercicios de la API con la DB 
        public async Task SincronizarEjerciciosAsync()
        {
            try
            {
                //Obtengo la ultima fecha de sincronizacion
                var ultimaSincronizacion = Preferences.Get("UltimaSyncEjercicios", DateTime.MinValue);

                // Si nunca se sincronizo y no hay conexion a internet
                if (ultimaSincronizacion == DateTime.MinValue && Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    // Muestro un alert informando que es necesario sincronizar al menos una vez
                    await _displayAlertService.MostrarAlertAsync("Sin conexión", "Necesitás sincronizar los ejercicios al menos una vez para usar la app. Intentá nuevamente cuando tengas internet.", "OK");
                    // bloqueo shell hasta que haya internet
                    (Application.Current?.Windows[0].Page as AppShell)?.PermitirNavegacionEnShell(false);
                    // Bucle para comprobar la conexion a internet
                    while (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    {
                        await Task.Delay(1000); // Espero 1 segundo antes de volver a verificar
                    }
                    // Cuando hay internet, permito la navegacion en shell
                    (Application.Current?.Windows[0].Page as AppShell)?.PermitirNavegacionEnShell(true);
                }
                else
                {
                    // Si la ultima sincronizacion fue hace menos de 24 horas, no hago nada y salgo de la task
                    if ((DateTime.Now - ultimaSincronizacion).TotalMinutes < 1)
                    {
                        return; // No es necesario sincronizar y salgo de la task
                    }

                    // Verifico conexion a internet
                    if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    {
                        // Si ya se sincronizo al menos una vez, muestro un alert informando que se esta viendo una version desactualizada
                        await _displayAlertService.MostrarAlertAsync("Sin conexión", $"No se pudieron sincronizar los ejercicios. Tene en cuenta que estas viendo una version desactualizada de los ejercicios y pueden cambiar al sincronizarse. Ultima sincronizacion {ultimaSincronizacion.ToString("dd/MM/yyyy")} ", "OK");
                        return;
                    }
                }

                // Capturo los ejercicios de la API y de la DB
                var listaEjerciciosApi = await _apiEjercicioService.GetEjerciciosAsync();
                var listaEjerciciosLocal = await _ejercicioRepositorio.ObtenerTodosLosEjerciciosAsync();
               
                // Si la tabla (DB) esta vacia, inserto todos los ejercicios de la API
                if (listaEjerciciosLocal.Count == 0)
                {
                    foreach(var e in listaEjerciciosApi)
                    {
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
            catch (Exception ex)
            {
                await _displayAlertService.MostrarAlertAsync("Error", ex.Message , "OK");
            }
        }
    }
}