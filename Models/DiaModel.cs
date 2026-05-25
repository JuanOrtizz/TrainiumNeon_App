using SQLite;

namespace TrainiumNeon.Models
{
    public class DiaModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(9)]
        public string Nombre { get; set; } = string.Empty;

        // Propiedad para la relacion con la rutina
        [Indexed]
        public int IdRutina { get; set; } // FK en la DB
    }
}
