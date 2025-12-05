using SQLite;
using System.Xml;
namespace TrainiumNeon.Models
{

    public class RutinaModel
    {
        // Propiedades de la rutina
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;
        // Propiedad para la relacion con el usuario
        [Indexed]
        public int IdUsuario { get; set; } // FK en la DB
        public bool EsSeleccionada { get; set; }
        public bool EsBorrador { get; set; }
        [Ignore]
        public int NumeroRutina { get; set; }
    }
}
