using SQLite;
using System.Security.Cryptography;
using TrainiumNeon.Models;

namespace TrainiumNeon.Data.Repositories
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        // Propiedad privada 
        private readonly SQLiteAsyncConnection _db;

        // Constructor
        public UsuarioRepositorio(DatabaseService db)
        {
            // Inicializa la DB y obtiene la conexion
            _db = db.Connection();
        }

        // Task asincrona para registrar un usuario
        public async Task<bool> RegistrarUsuarioAsync(string nombre, string email, string contrasenia)
        {
            //Creo el Salt y Hash de la contraseña
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            byte[] hash = HashContrasenia(contrasenia, salt);

            //Creo el usuario y lo inserto en la DB
            var usuario = new UsuarioModel {Nombre = nombre, Email = email, ContraseniaSalt = salt, ContraseniaHash = hash };
            int filas = await _db.InsertAsync(usuario);

            // Si se inserto (filas = 1) retorna true, sino false
            return filas > 0;
        }

        // Task asincrona para iniciar sesion
        public async Task<int> IniciarSesionAsync(string email, string contrasenia)
        {
            // Busco el usuario por email
            var usuario = await _db.Table<UsuarioModel>().Where(u => u.Email == email).FirstOrDefaultAsync();

            // Si no existe, retorno (Id = 0)
            if (usuario == null)
            {
                return 0;
            }

            // Capturo el hash de la contraseña ingresada
            byte[] hash = HashContrasenia(contrasenia, usuario.ContraseniaSalt);

            // Compruebo si el hash de la contraseña ingresada es igual a la almacenada. Si no coinciden, retorno (Id = 0)
            if (!hash.SequenceEqual(usuario.ContraseniaHash))
            {
                return 0;
            }

            // Si todo es correcto, retorno el usuario
            return usuario.Id;
        }

        // Task asincrona para verificar si existe un usuario con el email (Registro y actualizacion de usuario)
        public async Task<bool> ExisteUsuarioConEmailAsync(string email)
        {
            // Busco el usuario por email
            var usuario = await _db.Table<UsuarioModel>().Where(u => u.Email == email).FirstOrDefaultAsync();
            // Si no existe retorno false, sino true
            if (usuario == null)
            {
                return false;
            }
            return true;
        }

        // Task asincrona para obtener un usuario por Id
        public async Task<UsuarioModel> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            return await _db.Table<UsuarioModel>().Where(u => u.Id == idUsuario).FirstOrDefaultAsync();
        }

        // Task asincrona para Actualizar el nombre del usuario
        public async Task<bool> ActualizarNombreUsuarioAsync(int idUsuario, string nombre)
        {
            // Capturo el usuario
            var usuario = await _db.Table<UsuarioModel>().Where(u => u.Id == idUsuario).FirstOrDefaultAsync();
            // Si el nombre actual no es igual al ingresado lo actualiza
            if (usuario.Nombre != nombre)
            {
                usuario.Nombre = nombre;
                await _db.UpdateAsync(usuario);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Task asincrona para Actualizar el Email del usuario
        public async Task<bool> ActualizarEmailUsuarioAsync(int idUsuario, string email)
        {
            // Capturo el usuario
            var usuario = await _db.Table<UsuarioModel>().Where(u => u.Id == idUsuario).FirstOrDefaultAsync();
            // Si el email actual del usuario no es igual al ingresado lo actualiza
            if (usuario.Email != email)
            {
                usuario.Email = email;
                await _db.UpdateAsync(usuario);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Task asincrona para actualizar la contraseña del usuario
        public async Task<bool> ActualizarContraseniaUsuarioAsync(int idUsuario, string contrasenia)
        {
            // Capturo el usuario
            var usuario = await _db.Table<UsuarioModel>().Where(u => u.Id == idUsuario).FirstOrDefaultAsync();
            // Genero el nuevo salt
            byte[] nuevoSalt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(nuevoSalt);
            }
            // Genero el nuevo hash
            byte[] nuevoHash = HashContrasenia(contrasenia, nuevoSalt);
            // Actualizo los datos del usuario y lo actualizo en la DB
            usuario.ContraseniaSalt = nuevoSalt;
            usuario.ContraseniaHash = nuevoHash;
            int filas = await _db.UpdateAsync(usuario);
            return filas > 0;
        }

        // Metodo privado para Hashear la contraseña
        private byte[] HashContrasenia(string contrasenia, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(contrasenia, salt, 100_000, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(32); // 256bit hash
            }
        }
    }
}
