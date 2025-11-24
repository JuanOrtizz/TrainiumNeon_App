using SQLite;

namespace TrainiumNeon.Models
{
    public class EjercicioModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(100), Unique]
        public string Nombre { get; set; }
        // Propiedad para la relacion con el grupo muscular
        public int IdGrupoMuscular { get; set; } // FK en la DB
        public string? ImagenUrl { get; set; }
        public int PersonalRecord { get; set; } = 0;
    }
}
