using TrainiumNeon.Models;

namespace TrainiumNeon.Services
{
    public interface IApiEjerciciosService
    {
        Task<IReadOnlyList<EjercicioModel>> GetEjerciciosAsync();

        Task<EjercicioModel> GetEjercicioByIdAsync(int id);

        Task<IReadOnlyList<EjercicioModel>> GetEjerciciosByGrupoMuscularAsync(string grupoMuscular);
    }
}
