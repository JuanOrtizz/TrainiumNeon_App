namespace TrainiumNeon.Services
{
    public interface IValidacionService
    {
        string ValidarCampoVacio(string campo);
        string ValidarNombreCompleto(string nombreCompleto);
        string ValidarEmail(string email);
        string ValidarContrasenia(string contrasenia);
        string ValidarConfirmarContrasenia(string contrasenia, string confirmarContrasenia);
        string ValidarNombreRutina(string nombreRutina);
        string ValidarPR(string pr);
        string ValidarSeries(string series);
        string ValidarRepeticiones(string repeticiones);

    }
}
