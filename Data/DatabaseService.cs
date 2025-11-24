using SQLite;
using TrainiumNeon.Models;

namespace TrainiumNeon.Data
{
    public class DatabaseService
    {
        // Propiedades privadas
        private readonly SQLiteAsyncConnection _db;
        private bool _initialized;

        // Constructor
        public DatabaseService(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
        }

        // Metodo publico para obtener la conexion a la DB
        public SQLiteAsyncConnection Connection()
        {
            return _db;
        } 

        // Task asincrona para inicializar la DB
        public async Task InitializeAsync()
        {
            // Si la db ya esta inicializada salgo del metodo
            if (_initialized)
            {
                return;
            }

            // Creo las tablas de la DB
            await _db.CreateTableAsync<UsuarioModel>();
            await _db.CreateTableAsync<RutinaModel>();
            await _db.CreateTableAsync<EjercicioModel>();
            await _db.CreateTableAsync<DiaModel>();
            await _db.CreateTableAsync<EjercicioDiaModel>();
            await _db.CreateTableAsync<GrupoMuscularModel>();

            // Inicializo la variable para no volver a inicializar
            _initialized = true;
        }

    }
}
