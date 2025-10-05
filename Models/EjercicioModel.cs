using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainiumNeon.Models
{
    public class EjercicioModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string GrupoMuscular { get; set; }
        public string? ImagenUrl { get; set; }
        public int PersonalRecord { get; set; } = 0;
    }
}
