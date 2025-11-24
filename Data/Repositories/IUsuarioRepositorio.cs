using TrainiumNeon.Models;

namespace TrainiumNeon.Data.Repositories
{
    public interface IUsuarioRepositorio
    {
        // Autenticacion 
        Task<bool> RegistrarUsuarioAsync(string nombre, string email, string contrasenia);
        Task<int> IniciarSesionAsync(string email, string contrasenia);
        Task<bool> ExisteUsuarioConEmailAsync(string email);
        Task<UsuarioModel> ObtenerUsuarioPorIdAsync(int idUsuario);
        // Perfil
        Task<bool> ActualizarNombreUsuarioAsync(int idUsuario, string nombre);
        Task<bool> ActualizarEmailUsuarioAsync(int idUsuario, string email);
        Task<bool> ActualizarContraseniaUsuarioAsync(int idUsuario, string contrasenia);

    }
}
