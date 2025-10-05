using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainiumNeon.Models
{
    public class DiaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Propiedades para la relacion con la rutina
        public int RutinaId { get; set; } // va a ser la FK en la DB
        public RutinaModel Rutina { get; set; } // Objeto rutina al que pertenece el dia

        // Propiedad de lista de ejercicios (cada dia tiene una lista de ejercicios)
        public ICollection<EjercicioDiaModel> EjerciciosDia { get; set; }
    }
}
