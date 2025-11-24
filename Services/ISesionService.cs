namespace TrainiumNeon.Services
{
    public interface ISesionService
    {
        void GuardarSesion(int idUsuario);
        int ObtenerSesion();
        void CerrarSesion();
    }
}
