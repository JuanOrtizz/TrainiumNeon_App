using SQLite;
using TrainiumNeon.Models;

namespace TrainiumNeon.Data.Repositories
{
    public class RutinaRepositorio : IRutinaRepositorio
    {
        // Propiedad privada
        private readonly SQLiteAsyncConnection _db;

        // Delegate para patron observer
        public event Func<int, Task>? CreandoRutina;

        // Constructor
        public RutinaRepositorio(DatabaseService db)
        {
            // Inicializa la DB y obtiene la conexion
            _db = db.Connection();
        }

        // Task asincrona para crear una rutina vacia (Funciona como borrador)
        public async Task<RutinaModel> CrearRutinaVaciaAsync(int idUsuario)
        {
            // Crea un objeto rutina con nombre nulo, y la inicializa como borrador (EsSeleccionada false porque todavia no tiene un registro logico ya que es borrador)
            var rutina = new RutinaModel
            {
                IdUsuario = idUsuario,
                EsSeleccionada = false,
                EsBorrador = true
            };

            // Inserta en la rutina en la DB
            await _db.InsertAsync(rutina);

            // Si tiene suscriptores ejecuta todos los metodos suscriptos al evento de registro (CrearDiasAsync)
            if (CreandoRutina != null)
            {
                await CreandoRutina.Invoke(rutina.Id);
            }

            // retorno el id de la rutina
            return rutina;

        }

        // Task asincrona para obtener rutina por su Id
        public async Task<RutinaModel> ObtenerRutinaPorIdAsync(int idRutina)
        {
            return await _db.FindAsync<RutinaModel>(idRutina);
        }

        // Task asincrona para obtener rutina seleccionada
        public async Task<RutinaModel> ObtenerRutinaSeleccionadaAsync(int idUsuario)
        {
            return await _db.Table<RutinaModel>().Where(r => r.IdUsuario == idUsuario && r.EsSeleccionada).FirstOrDefaultAsync();
        }

        // Task asincrona para obtener todas las rutinas de un usuario
        public async Task<IReadOnlyList<RutinaModel>> ObtenerRutinasPorUsuarioAsync(int idUsuario)
        {
            return await _db.Table<RutinaModel>().Where(r => r.IdUsuario == idUsuario && !r.EsBorrador).ToListAsync();
        }

        // Task asincrona para obtener la rutina guardada como borrador
        public async Task<RutinaModel> ObtenerRutinaEnBorradorAsync(int idUsuario)
        {
            return await _db.Table<RutinaModel>().Where(r => r.IdUsuario == idUsuario && r.EsBorrador).FirstOrDefaultAsync();
        }

        // Task asincrona para Actualizar la rutina vacia (Creada como borrador) al establecer un nombre
        public async Task<bool> ActualizarRutinaVaciaAsync(int idRutina, int idUsuario, string nombreRutina)
        {
            // capturo la rutina
            var rutina = await _db.Table<RutinaModel>().Where(r => r.Id == idRutina).FirstOrDefaultAsync();
            // Actualizo el nombre y la registro logicamente poniendo EsBorrador = false
            rutina.Nombre = nombreRutina;
            rutina.EsBorrador = false;
            // capturo las rutinas y veo si ya existe alguna. Si no existe se marca como RutinaSeleccionada ya que es la primera
            var contadorRutinas = await _db.Table<RutinaModel>().Where(r => r.IdUsuario == idUsuario).CountAsync();
            if (contadorRutinas == 1)
            {
                rutina.EsSeleccionada = true;
            }
            // Actualizo la rutina en la DB
            var filas = await _db.UpdateAsync(rutina);
            // Devuelvo si se produjo la actualizacion o no
            return filas > 0;
        }

        // Task asincrona para actualizar el nombre de la rutina 
        public async Task<bool> ActualizarNombreRutinaAsync(int idRutina, string nuevoNombreRutina)
        {
            // Capturo la rutina
            var rutina = await _db.Table<RutinaModel>().Where(r => r.Id == idRutina).FirstOrDefaultAsync();
            // cambio el nombre de la rutina
            rutina.Nombre = nuevoNombreRutina;
            // Actualizo la rutina en la DB
            var filas = await _db.UpdateAsync(rutina);
            // Devuelvo si se produjo la actualizacion o no
            return filas > 0;
        }

        // Task Asincrona para eliminar la rutina (En cascada se eliminan sus dias y los ejercicios por dia)
        public async Task<bool> EliminarRutinaAsync(int idRutina)
        {
            //Obtengo todo los dias de la rutina
            var diasRutina = await _db.Table<DiaModel>().Where(d => d.IdRutina == idRutina).ToListAsync();

            // Recorro los dias de la rutina, obtengo los ejercicios por dia y elimino los ejercicios por dia y los dias
            foreach (var dia in diasRutina)
            {
                var ejerciciosDias = await _db.Table<EjercicioDiaModel>().Where(ed => ed.IdDia == dia.Id).ToListAsync();
                foreach(var ed in ejerciciosDias)
                {
                    await _db.DeleteAsync(ed);
                }
                await _db.DeleteAsync(dia);
            }

            // capturo la rutina
            var rutina = await _db.Table<RutinaModel>().Where(r => r.Id == idRutina) .FirstOrDefaultAsync();
            // Elimino la rutina finalmente
            var filas = await _db.DeleteAsync(rutina);

            //Actualizo la rutina seleccionada a la ultima rutina creada
            return filas > 0;

        }

        // Task asincrona para marcar una rutina como seleccionada
        public async Task<bool> MarcarRutinaSeleccionadaAsync(int idRutina)
        {
            // capturo la rutina
            var rutina = await _db.Table<RutinaModel>().Where(r => r.Id == idRutina).FirstOrDefaultAsync();
            // Hago un update de la rutina seleccionada de un usuario para desmarcarla en la DB
            var filasActualizadas = await _db.ExecuteAsync("UPDATE RutinaModel SET EsSeleccionada = 0 WHERE IdUsuario = ?",rutina.IdUsuario);
            // Actualizo la rutina como seleccionada
            rutina.EsSeleccionada = true;
            // Actualizo la rutina (Nueva seleccionada) en la DB
            var filas = await _db.UpdateAsync(rutina);
            // Devuelvo si se produjo la actualizacion o no
            return filas > 0;
        }

        // Task asincrona para verificar si una rutina es seleccionada
        public async Task<bool> EsRutinaSeleccionada(int idUsuario, int idRutina)
        {
            var contador = await _db.Table<RutinaModel>().Where(r => r.IdUsuario == idUsuario && r.Id == idRutina && r.EsSeleccionada).CountAsync();
            return contador > 0;
        }

        // Task asincrona para obtener si hay un borrador de rutina (Rutina no guardada)
        public async Task<bool> ExisteRutinaEnBorradorAsync(int idUsuario)
        {
            // Realizo un conteo de todas las rutinas donde EsBorrador es True (Siempre va a ser 1 o 0)
            var contador = await _db.Table<RutinaModel>().Where(r => r.IdUsuario == idUsuario && r.EsBorrador).CountAsync();
            return contador > 0; // Devuelvo el valor booleano 
        }

        // Task asincrona para verificar si no existe una rutina con el nombre cargado (Para evitar rutinas duplicadas)
        public async Task<bool> ExisteRutinaConNombreAsync(int idUsuario, string nombreRutina)
        {
            // capturo la rutina con el nombre (Si no hay devuelve false [ya que es null], sino devuelvo true, indicando que ya hay una rutina con ese nombre)
            var rutina = await _db.Table<RutinaModel>().Where(r => r.Nombre == nombreRutina && r.IdUsuario == idUsuario).FirstOrDefaultAsync();
            return rutina != null;
        }
    }
}
