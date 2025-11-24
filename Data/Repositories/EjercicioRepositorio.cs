using SQLite;
using TrainiumNeon.Models;

namespace TrainiumNeon.Data.Repositories
{
    public class EjercicioRepositorio : IEjercicioRepositorio
    {
        // Propiedad privada
        private readonly SQLiteAsyncConnection _db;

        // Constructor
        public EjercicioRepositorio(DatabaseService db)
        {
            // Inicializa la DB y obtiene la conexion
            _db = db.Connection();
        }

        // Task asincrona para crear un ejercicio
        public async Task CrearEjercicioAsync(string nombre, int idGrupoMuscular, string? imagenPath)
        {
            // crea el objeto ejercicio
            var ejercicio = new EjercicioModel { Nombre = nombre, IdGrupoMuscular = idGrupoMuscular, ImagenUrl = imagenPath };
            // guardo el ejercicio en la DB
            await _db.InsertAsync(ejercicio);
        }

        // Task asincrona para obtener un ejercicio por su id
        public async Task<EjercicioModel> ObtenerEjercicioPorIdAsync(int idEjercicio)
        {
            return await _db.Table<EjercicioModel>().Where(e => e.Id == idEjercicio).FirstOrDefaultAsync();
        }

        // Task asincrona para obtener ejercicios filtrados por grupos muscular
        public async Task<IReadOnlyList<EjercicioModel>> ObtenerEjerciciosPorGrupoMuscularAsync(int idGrupoMuscular)
        {
            return await _db.Table<EjercicioModel>().Where(e => e.IdGrupoMuscular == idGrupoMuscular).ToListAsync();
        }

        // Task asincrona para obtener todos los ejercicios
        public async Task<IReadOnlyList<EjercicioModel>> ObtenerTodosLosEjerciciosAsync()
        {
            return await _db.Table<EjercicioModel>().ToListAsync();
        }

        // Task asincrona para ordenar los ejercicios (Filtrado)
        public async Task<IReadOnlyList<EjercicioModel>> OrdenarEjerciciosAsync(string ordenamiento, int idGrupoMuscular)
        {
            // Declaro la variable ejercicios (Lista que se va a devolver)
            List<EjercicioModel> ejercicios;
            // Si el grupo muscular es 0 (Todos), trae todos los ejercicios
            if (idGrupoMuscular == 0)
            {
                ejercicios = await _db.Table<EjercicioModel>().ToListAsync();
            }
            else // Si hay otro grupo muscular seleccionado
            {
                // traigo ejercicios por el grupo muscular seleccionado
                ejercicios = await _db.Table<EjercicioModel>().Where(e => e.IdGrupoMuscular == idGrupoMuscular).ToListAsync();
            }

            // Switch para ordenar los ejercicios
            switch (ordenamiento.ToUpper().Trim())
            {
                case "ABC ASC": ejercicios = ejercicios.OrderBy(e => e.Nombre).ToList(); break;
                case "ABC DESC": ejercicios = ejercicios.OrderByDescending(e => e.Nombre).ToList(); break;
                case "PR ASC": ejercicios = ejercicios.OrderBy(e => e.PersonalRecord).ToList(); break;
                case "PR DESC": ejercicios = ejercicios.OrderByDescending(e => e.PersonalRecord).ToList(); break;
            }

            // Devuelvo los ejercicios ordenados y filtrados por grupo muscular seleccionado
            return ejercicios;
        }

        // Task asincrona para actualizar un ejercicio
        public async Task ActualizarEjercicioAsync(int idEjercicio, int idNuevoGrupoMuscular, string? nuevaImagenUrl)
        {
            // Obtengo el ejercicio a actualizar
            var ejercicio = await _db.Table<EjercicioModel>().Where(e => e.Id == idEjercicio).FirstOrDefaultAsync();
            // Le asigno los nuevos valores 
            ejercicio.IdGrupoMuscular = idNuevoGrupoMuscular;
            ejercicio.ImagenUrl = nuevaImagenUrl;
            // Actualizo el ejercicio
            await _db.UpdateAsync(ejercicio);
        }

        // Task asincrona para actualizar el personal record de un ejercicio
        public async Task<bool> ActualizarPersonalRecordAsync(int idEjercicio, int nuevoPersonalRecord)
        {
            // Capturo el ejercicio a actualizar
            var ejercicio = await _db.Table<EjercicioModel>().Where(e => e.Id == idEjercicio).FirstOrDefaultAsync();
            // Actualizo su personal record
            ejercicio.PersonalRecord = nuevoPersonalRecord;
            // Actualizo el ejercicio en la DB
            var filas = await _db.UpdateAsync(ejercicio);
            // Devuelvo si se actualizo o no
            return filas > 0;
        }

    }
}
