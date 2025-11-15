using SQLite;

namespace TrainiumNeon.Models
{
    public class DiaModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(9)]
        public string Nombre { get; set; }

        // Propiedades para la relacion con la rutina
        [Indexed]
        public int RutinaId { get; set; } // va a ser la FK en la DB
        [Ignore]
        public RutinaModel Rutina { get; set; } // Objeto rutina al que pertenece el dia

        // Propiedad de lista de ejercicios (cada dia tiene una lista de ejercicios)
        [Ignore]
        public ICollection<EjercicioDiaModel> EjerciciosDia { get; set; }
    }
}
