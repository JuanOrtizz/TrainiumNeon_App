using SQLite;
namespace TrainiumNeon.Models
{
    public class RutinaModel
    {
        // Propiedades de la rutina
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Nombre { get; set; }

        // Propiedades para la relacion con el usuario
        [Indexed]
        public int IdUsuario { get; set; } // va a ser la FK en la DB
        [Ignore]
        public UsuarioModel Usuario { get; set; } // Objeto usuario al que pertenece la rutina

        //Propiedad de lista de dias (Lunes a Domingo, cada dia tiene una lista de ejercicios)
        [Ignore]
        public ICollection<DiaModel> Dias { get; set; }
    }
}
