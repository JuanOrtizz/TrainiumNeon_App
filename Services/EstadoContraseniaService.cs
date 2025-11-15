using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainiumNeon.Services
{
    public class EstadoContraseniaService : IEstadoContraseniaService
    {
        public (bool contraseniaOculta, string iconoContrasenia) CambiarEstadoContrasenia(bool contraseniaOculta, string iconoContrasenia)
        {
            contraseniaOculta = !contraseniaOculta;
            if (contraseniaOculta)
            {
                iconoContrasenia = "ver_contrasenia.png";
            }
            else
            {
                iconoContrasenia = "no_ver_contrasenia.png";
            }
            return (contraseniaOculta, iconoContrasenia);
        }
    }
}
