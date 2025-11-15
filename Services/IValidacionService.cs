namespace TrainiumNeon.Services
{
    public interface IValidacionService
    {
        string ValidarEmail(string email);
        string ValidarContrasenia(string contrasenia);
        string ValidarConfirmarContrasenia(string contrasenia, string confirmarContrasenia);
        string ValidarNombreCompleto(string nombreCompleto);
        string ValidarPR(string pr);
        string ValidarNombreRutina(string nombreRutina);
        string ValidarEjercicioSeleccionado(string ejercicio);
        string ValidarSeries(string series);
        string ValidarRepeticiones(string repeticiones);

    }
}
