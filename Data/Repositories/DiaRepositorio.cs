using SQLite;
using TrainiumNeon.Models;

namespace TrainiumNeon.Data.Repositories
{
    public class DiaRepositorio : IDiaRepositorio
    {
        // Propiedad privada
        private readonly SQLiteAsyncConnection _db;

        // Constructor 
        public DiaRepositorio(DatabaseService db)
        {
            // Inicializa la DB y obtiene la conexion
            _db = db.Connection();
        }

        // Task asincrona para crear dias al crear una rutina (Con patron observer y un Func<int,Task>?)
        public async Task CrearDiasAsync(int idRutina)
        {
            // Creo la lista con los dias de la semana para la rutina
            var diasSemana = new List<string>
            {
                "Lunes",
                "Martes",
                "Miércoles",
                "Jueves",
                "Viernes",
                "Sábado",
                "Domingo"
            };
            // Recorro la lista, creo un objeto Dia y lo inserto en la DB
            foreach (var nombreDia in diasSemana)
            {
                var dia = new DiaModel
                {
                    Nombre = nombreDia,
                    IdRutina = idRutina
                };
                await _db.InsertAsync(dia);
            }
        }

        // Task asincrona para obtener dia por Id
        public async Task<DiaModel> ObtenerDiaPorIdAsync(int idDia)
        {
            return await _db.Table<DiaModel>().Where(d => d.Id == idDia).FirstOrDefaultAsync();
        }

        // Task asincrona para obtener dia por Nombre
        public async Task<DiaModel> ObtenerDiaPorNombreAsync(int idRutina, string nombre)
        {
            return await _db.Table<DiaModel>().Where(d => d.IdRutina == idRutina && d.Nombre == nombre).FirstOrDefaultAsync();
        }

        // Task asincrona para obtener todos los dias de una rutina
        public async Task<IReadOnlyList<DiaModel>> ObtenerDiasPorRutinaAsync(int idRutina)
        {
            return await _db.Table<DiaModel>().Where(d => d.IdRutina == idRutina).ToListAsync();
        }
    }
}
