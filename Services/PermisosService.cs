
namespace TrainiumNeon.Services
{
    public class PermisosService : IPermisosService
    {
        // Task asincrona para solicitar el permiso de ubicacion
        public async Task<bool> SolicitarUbicacionAsync()
        {
            // verifico el estado del permiso
            var estado = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            // Si no lo concedio lo pido
            if (estado != PermissionStatus.Granted)
            {
                estado = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            // Retorno el estado
            return estado == PermissionStatus.Granted;
        }

        public async Task<bool> SolicitarPermisosNotificacionesAsync()
        {
        // Hago asi ya que Android 13+ debe pedir PostNotifications
        if (OperatingSystem.IsAndroidVersionAtLeast(33))
        {
            var estado = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

            if (estado != PermissionStatus.Granted)
            {
                estado = await Permissions.RequestAsync<Permissions.PostNotifications>();
            }

            return estado == PermissionStatus.Granted;
        }

        // Android 12 o menor NO necesita pedir permiso por eso devuelvo true
        return true;
        }
    }

}