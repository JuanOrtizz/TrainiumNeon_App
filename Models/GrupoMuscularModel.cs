using SQLite;
namespace TrainiumNeon.Models
{
    public class GrupoMuscularModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(15), Unique]
        public string Nombre { get; set; } = string.Empty;
    }
}
