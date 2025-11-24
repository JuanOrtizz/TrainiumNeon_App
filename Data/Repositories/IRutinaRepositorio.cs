using TrainiumNeon.Models;

namespace TrainiumNeon.Data.Repositories
{
    public interface IRutinaRepositorio
    {

        // evento para patron observer
        event Func<int, Task>? CreandoRutina;

        // Tasks
        Task<RutinaModel> CrearRutinaVaciaAsync(int idUsuario);
        Task<RutinaModel> ObtenerRutinaPorIdAsync(int idRutina);
        Task<RutinaModel> ObtenerRutinaSeleccionadaAsync(int idUsuario);
        Task<IReadOnlyList<RutinaModel>> ObtenerRutinasPorUsuarioAsync(int idUsuario);
        Task<RutinaModel> ObtenerRutinaEnBorradorAsync(int idUsuario);
        Task<bool> ActualizarRutinaVaciaAsync(int idRutina, int idUsuario, string nombreRutina);
        Task<bool> ActualizarNombreRutinaAsync(int idRutina, string nuevoNombreRutina);
        Task<bool> EliminarRutinaAsync(int idRutina);
        Task<bool> MarcarRutinaSeleccionadaAsync(int idRutina);
        Task<bool> EsRutinaSeleccionada(int idUsuario, int idRutina);
        Task<bool> ExisteRutinaEnBorradorAsync(int idUsuario);
        Task<bool> ExisteRutinaConNombreAsync(int idUsuario, string nombreRutina);

    }
}
