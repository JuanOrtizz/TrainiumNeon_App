namespace TrainiumNeon.Data.Repositories
{
    public interface IEstadisticasRepositorio
    {
        Task<int> ObtenerCantidadDeAparicionesEnRutinasAsync(int idEjercicio);
        // Se van a agregar nuevas estadisticas como cantidad de ejercicios por dia, cantidad de ejercicios por semana, Ultimos PRs actualizados, etc
    }
}
