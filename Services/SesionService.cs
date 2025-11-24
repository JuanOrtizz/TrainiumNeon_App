namespace TrainiumNeon.Services
{
    public class SesionService : ISesionService
    {
        // Clave para almacenar el ID de usuario en las preferencias
        private const string IdUsuarioKey = "IdUsuarioActivo";

        // Metodo para guardar la sesion del usuario al iniciar sesion en Preferences
        public void GuardarSesion(int idUsuario)
        {
            Preferences.Set(IdUsuarioKey, idUsuario);
        }

        // Metodo para obtener la sesion del usuario desde preferences
        public int ObtenerSesion()
        {
            // Verifica si hay un ID de usuario almacenado
            if (Preferences.ContainsKey(IdUsuarioKey))
            {
                // si hay retorno el ID de usuario
                int id = Preferences.Get(IdUsuarioKey, 0);
                if (id > 0)
                {
                    return id;
                }

                return 0;
            }
            // Si no hay usuario activo retorno 0
            return 0;
        }

        // Metodo para cerrar la sesion del usuario desde preferences
        public void CerrarSesion()
        {
            Preferences.Remove(IdUsuarioKey);
        }
    }
}
