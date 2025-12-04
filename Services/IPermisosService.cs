namespace TrainiumNeon.Services
{
    public interface IPermisosService
    {
        Task<bool> SolicitarUbicacionAsync();
        Task<bool> SolicitarPermisosNotificacionesAsync();
    }
}
