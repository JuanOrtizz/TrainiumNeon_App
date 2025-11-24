namespace TrainiumNeon.Services
{
    public class EstadoContraseniaService : IEstadoContraseniaService
    {
        // Metodo para cambiar el estado e icono de la contraseña (Mostrar/Ocultar)
        public (bool contraseniaOculta, string iconoContrasenia) CambiarEstadoContrasenia(bool contraseniaOculta, string iconoContrasenia)
        {
            // Cambio el valor booleano actual del estado
            contraseniaOculta = !contraseniaOculta;
            // Si esta oculta, muestro icono para ver
            if (contraseniaOculta)
            {
                iconoContrasenia = "ver_contrasenia.png";
            }
            else // Si se esta mostrando, muestro icono para ocultar
            {
                iconoContrasenia = "no_ver_contrasenia.png";
            }
            // Retorno el estado actual y el icono correspondiente
            return (contraseniaOculta, iconoContrasenia);
        }
    }
}
