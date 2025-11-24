using TrainiumNeon.Models;

namespace TrainiumNeon.Data.Repositories
{
    public interface IEjercicioDiaRepositorio
    {
        Task<bool> AgregarEjercicioADiaAsync(int idDia, int idEjercicio, int series, int repeticiones);
        Task<EjercicioDiaModel> ObtenerEjercicioDiaPorIdAsync(int idEjercicioDia);
        Task<IReadOnlyList<EjercicioDiaModel>> ObtenerEjerciciosPorDiaAsync(int idDia);
        Task<bool> ActualizarEjercicioDiaAsync(int idEjercicioDia, int idDia, int series, int repeticiones);
        Task<bool> EliminarEjercicioDiaAsync(int idEjercicioDia);
        Task<bool> ExisteEjercicioDiaEnDiaAsync(int idDia, int idEjercicio);
    }
}
