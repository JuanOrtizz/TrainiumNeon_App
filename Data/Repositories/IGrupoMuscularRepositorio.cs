using TrainiumNeon.Models;

namespace TrainiumNeon.Data.Repositories
{
    public interface IGrupoMuscularRepositorio
    {
        Task RegistrarGruposMuscularesAsync();
        Task<GrupoMuscularModel> ObtenerGrupoMuscularPorIdAsync(int idGrupoMuscular);
        Task<GrupoMuscularModel> ObtenerGrupoMuscularPorNombreAsync(string nombreGrupoMuscular);
        Task<IReadOnlyList<GrupoMuscularModel>> ObtenerTodoGruposMuscularesAsync();
    }
}
