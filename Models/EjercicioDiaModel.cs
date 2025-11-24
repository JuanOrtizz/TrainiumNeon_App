using SQLite;

namespace TrainiumNeon.Models
{
    //Clase para representar la relacion entre EjercicioModel y DiaModel (Un ejericicio puede estar en muchos dias con diferentes series y repeticiones)
    public class EjercicioDiaModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // Propiedades para la relacion con el ejercicio
        [Indexed]
        public int IdEjercicio { get; set; } // va a ser la FK en la DB
        [Ignore]
        public EjercicioModel Ejercicio { get; set; } // Objeto ejercicio al que pertenece el ejercicio del dia

        // Propiedad para la relacion con el dia
        [Indexed]
        public int IdDia { get; set; } // FK en la DB

        //Propiedades para series y repeticiones por dia del ejercicio
        public int Series { get; set; }
        public int Repeticiones { get; set; }

    }
}
