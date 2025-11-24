using TrainiumNeon.Models;

namespace TrainiumNeon.Services
{
    public interface IApiEjerciciosService
    {
        Task<IReadOnlyList<EjercicioModel>> GetEjerciciosAsync();
    }
}
