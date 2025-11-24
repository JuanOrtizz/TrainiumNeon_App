using SQLite;
using TrainiumNeon.Models;

namespace TrainiumNeon.Data.Repositories
{
    public class EstadisticasRepositorio : IEstadisticasRepositorio
    {
        // Propiedad privada
        private readonly SQLiteAsyncConnection _db;

        //Constructor
        public EstadisticasRepositorio(DatabaseService db) 
        { 
            // Inicializa la DB y obtiene la conexion
            _db = db.Connection();
        }

        // Task asincrona para obtener la cantidad de apariciones del ejercicio en rutinas
        public async Task<int> ObtenerCantidadDeAparicionesEnRutinasAsync(int idEjercicio)
        {
            //Obtengo todos los ejerciciosDia que sean de ese ejercicio
            var ejerciciosDia = await _db.Table<EjercicioDiaModel>().Where(ed => ed.IdEjercicio == idEjercicio).ToListAsync();

            //Obtenngo todos los idDia distintos de cada ejercicio
            var idDias = ejerciciosDia.Select(ed => ed.IdDia).Distinct().ToList();

            // Obtengo los dias con esos ids
            var dias = await _db.Table<DiaModel>().Where(d => idDias.Contains(d.Id)).ToListAsync();

            // cuento la cantidad de rutinas que contienen el ejercicio gracias a los dias
            var cantidadRutinas = dias.Select(d => d.IdRutina).Distinct().Count();

            // Devuelvo la cantidad de rutinas
            return cantidadRutinas;
        }
    }
}
