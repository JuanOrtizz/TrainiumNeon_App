using SQLite;

namespace TrainiumNeon.Models
{
    public class UsuarioModel
    {
        //Propiedades del usuario
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Nombre { get; set; }
        [MaxLength(254), Unique]
        public string Email { get; set; }

        //Propiedades para contraseña con seguridad (Hash y Salt)
        public byte[] ContraseniaHash { get; set; }
        public byte[] ContraseniaSalt { get; set; }
    }
}
