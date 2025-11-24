using SQLite;
using TrainiumNeon.Models;

namespace TrainiumNeon.Data.Repositories
{
    public class EjercicioDiaRepositorio : IEjercicioDiaRepositorio
    {
        // Propiedad privada
        private readonly SQLiteAsyncConnection _db;

        // Constructor
        public EjercicioDiaRepositorio(DatabaseService db)
        {
            // Inicializa la DB y obtiene la conexion
            _db = db.Connection();
        }

        // Task asincrona para agregar un ejercicio a un dia de una rutina
        public async Task<bool> AgregarEjercicioADiaAsync(int idDia, int idEjercicio, int series, int repeticiones)
        {
            // Creo el objeto EjercicioDia
            var ejercicioDia = new EjercicioDiaModel
            {
                IdDia = idDia,
                IdEjercicio = idEjercicio,
                Series = series,
                Repeticiones = repeticiones
            };
            // Registro el ejercicio en la DB
            var filas = await _db.InsertAsync(ejercicioDia);
            // Devuelvo si se registro o no
            return filas > 0;
        }

        // Task asincrona para obtener el ejercicioDia por Id
        public async Task<EjercicioDiaModel> ObtenerEjercicioDiaPorIdAsync(int idEjercicioDia)
        {
            return await _db.Table<EjercicioDiaModel>().Where(ed => ed.Id == idEjercicioDia).FirstOrDefaultAsync();
        }

        // Task asincrona para obtener los Ejercicios por dia
        public async Task<IReadOnlyList<EjercicioDiaModel>> ObtenerEjerciciosPorDiaAsync(int idDia)
        {
            return await _db.Table<EjercicioDiaModel>().Where(ed => ed.IdDia == idDia).ToListAsync();
        }

        // Task asincrona para actualizar un ejercicioDia
        public async Task<bool> ActualizarEjercicioDiaAsync(int idEjercicioDia, int idDia, int series, int repeticiones)
        {
            // Capturo el ejercicioDia a actualizar
            var ejercicioDia = await _db.Table<EjercicioDiaModel>().Where(ed => ed.Id == idEjercicioDia).FirstOrDefaultAsync();
            // Actualizo sus valores
            ejercicioDia.IdDia = idDia;
            ejercicioDia.Series = series;
            ejercicioDia.Repeticiones = repeticiones;
            // Actualizo en la DB
            var filas = await _db.UpdateAsync(ejercicioDia);
            // Devuelvo si se actualizo o no
            return filas > 0;
        }

        // Task asincrona para eliminar un EjercicioDia
        public async Task<bool> EliminarEjercicioDiaAsync(int idEjercicioDia)
        {
            // Capturo el ejercicio
            var ejercicioDia = await _db.Table<EjercicioDiaModel>().Where(ed => ed.Id == idEjercicioDia).FirstOrDefaultAsync();
            // Elimino el ejercicio de la DB
            var filas = await _db.DeleteAsync(ejercicioDia);
            // Devuelvo si se elimino o no
            return filas > 0;
        }

        // Task asincrona para verificar que no exista un ejercicioDia para un dia
        public async Task<bool> ExisteEjercicioDiaEnDiaAsync(int idDia, int idEjercicio)
        {
            var contador = await _db.Table<EjercicioDiaModel>().Where(ed => ed.IdDia == idDia && ed.IdEjercicio == idEjercicio).CountAsync();
            return contador > 0;
        }

    }
}
