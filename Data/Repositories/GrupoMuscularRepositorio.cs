using SQLite;
using TrainiumNeon.Models;

namespace TrainiumNeon.Data.Repositories
{
    public class GrupoMuscularRepositorio : IGrupoMuscularRepositorio
    {
        // Propiedad privada
        private readonly SQLiteAsyncConnection _db;

        // Constructor
        public GrupoMuscularRepositorio(DatabaseService db)
        {
            // Inicializa la DB y obtiene la conexion
            _db = db.Connection();
        }

        // Task asincrona para registrar los grupos musculares (Se insertan al crear la DB desde MauiProgram.cs)
        public async Task RegistrarGruposMuscularesAsync()
        {
            // declaro la lista de grupos musculares
            var gruposMusculares = new List<string>
            {
                "Pecho",
                "Espalda",
                "Bíceps",
                "Tríceps",
                "Hombro",
                "Pierna",
                "Core"
            };

            // recorro con un foreach cada grupo muscular de la lista 
            foreach (var gm in gruposMusculares)
            {
                // obtengo el Grupo muscular
                var existente = await _db.Table<GrupoMuscularModel>() .Where(g => g.Nombre == gm).FirstOrDefaultAsync();
                // si es null (no existe) creo un objeto GrupoMuscular y lo inserto a la DB
                if (existente == null)
                {
                    var nuevoGrupo = new GrupoMuscularModel { Nombre = gm };
                    await _db.InsertAsync(nuevoGrupo);
                }
            }
        }

        // Task asincrona para obtener un grupo muscular por su Id
        public async Task<GrupoMuscularModel> ObtenerGrupoMuscularPorIdAsync(int idGrupoMuscular)
        {
            return await _db.Table<GrupoMuscularModel>().Where(g => g.Id == idGrupoMuscular).FirstOrDefaultAsync();
        }

        // Task asincrona para obtener un grupo muscular por su nombre
        public async Task<GrupoMuscularModel> ObtenerGrupoMuscularPorNombreAsync(string nombreGrupoMuscular)
        {
            return await _db.Table<GrupoMuscularModel>().Where(g => g.Nombre == nombreGrupoMuscular).FirstOrDefaultAsync();
        }

        // Task asincrona para obtener todos los grupos musculares
        public async Task<IReadOnlyList<GrupoMuscularModel>> ObtenerTodoGruposMuscularesAsync()
        {
            return await _db.Table<GrupoMuscularModel>().ToListAsync();
        }

    }
}
