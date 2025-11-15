namespace TrainiumNeon.Models
{
    public class UsuarioModel
    {
        //Propiedades del usuario
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }

        //Propiedades para contraseña con seguridad (Hash y Salt)
        public byte[] ContraseniaHash { get; set; }
        public byte[] ContraseniaSalt { get; set; }

        //Propiedad para la lista de rutinas
        public ICollection<RutinaModel> Rutinas { get; set; }
    }
}
