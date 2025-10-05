using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainiumNeon.Models
{
    //Clase para representar la relacion * a * entre EjercicioModel y DiaModel (Un ejericicio puede estar en muchos dias con diferentes series y repeticiones)
    public class EjercicioDiaModel
    {
        public int Id { get; set; }

        // Propiedades para la relacion con el ejercicio
        public int EjercicioId { get; set; } // va a ser la FK en la DB
        public EjercicioModel Ejercicio { get; set; } // Objeto ejercicio al que pertenece el ejercicio del dia

        // Propiedades para la relacion con el dia
        public int DiaId { get; set; } // va a ser la FK en la DB
        public DiaModel Dia { get; set; } // Objeto dia al que pertenece el ejercicio del dia

        //Propiedades para series y repeticiones por dia del ejercicio
        public string Series { get; set; }
        public string Repeticiones { get; set; }

    }
}
