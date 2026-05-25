using SQLite;

namespace TrainiumNeon.Models
{
    public class UsuarioModel
    {
        //Propiedades del usuario
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [MaxLength(254), Unique]
        public string Email { get; set; } = string.Empty;

        //Propiedades para contraseña con seguridad (Hash y Salt)
        public byte[] ContraseniaHash { get; set; } = new byte[0];
        public byte[] ContraseniaSalt { get; set; } = new byte[0];
    }
}
