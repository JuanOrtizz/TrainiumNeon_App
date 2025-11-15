using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainiumNeon.Services
{
    public interface IEstadoContraseniaService
    {
        (bool contraseniaOculta, string iconoContrasenia) CambiarEstadoContrasenia(bool contraseniaOculta, string iconoContrasenia);
    }
}
