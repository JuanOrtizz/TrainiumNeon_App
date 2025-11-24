using TrainiumNeon.Models;

namespace TrainiumNeon.Data.Repositories
{
    public interface IEjercicioRepositorio
    {
        Task CrearEjercicioAsync(string nombre, int idGrupoMuscular, string? imagenPath);
        Task<EjercicioModel> ObtenerEjercicioPorIdAsync(int idEjercicio);
        Task<IReadOnlyList<EjercicioModel>> ObtenerEjerciciosPorGrupoMuscularAsync(int idGrupoMuscular);
        Task<IReadOnlyList<EjercicioModel>> ObtenerTodosLosEjerciciosAsync();
        Task<IReadOnlyList<EjercicioModel>> OrdenarEjerciciosAsync(string ordenamiento, int idGrupoMuscular);
        Task ActualizarEjercicioAsync(int idEjercicio, int idNuevoGrupoMuscular, string? nuevaImagenUrl);
        Task<bool> ActualizarPersonalRecordAsync(int idEjercicio, int nuevoPersonalRecord);
    }
}
