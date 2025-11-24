using TrainiumNeon.Models;

namespace TrainiumNeon.Data.Repositories
{
    public interface IDiaRepositorio
    {
        Task CrearDiasAsync(int idRutina);
        Task<DiaModel> ObtenerDiaPorIdAsync(int idDia);
        Task<DiaModel> ObtenerDiaPorNombreAsync(int idRutina, string nombre);
        Task<IReadOnlyList<DiaModel>> ObtenerDiasPorRutinaAsync(int idRutina);
    }
}
