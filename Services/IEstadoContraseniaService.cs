namespace TrainiumNeon.Services
{
    public interface IEstadoContraseniaService
    {
        (bool contraseniaOculta, string iconoContrasenia) CambiarEstadoContrasenia(bool contraseniaOculta, string iconoContrasenia);
    }
}
