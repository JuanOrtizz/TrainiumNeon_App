namespace TrainiumNeon.Services
{
    public interface INotificacionService
    {
        void EnviarNotificacion(string titulo, string mensaje, DateTime? programarHorario = null, bool repetirDiariamente = false);
    }
}
