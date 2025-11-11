using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainiumNeon.Models
{
    public class RutinaModel
    {
        // Propiedades de la rutina
        public int Id { get; set; }
        public string Nombre { get; set; }
        
        // Propiedades para la relacion con el usuario
        public int IdUsuario { get; set; } // va a ser la FK en la DB
        public UsuarioModel Usuario { get; set; } // Objeto usuario al que pertenece la rutina

        //Propiedad de lista de dias (Lunes a Domingo, cada dia tiene una lista de ejercicios)
        public ICollection<DiaModel> Dias { get; set; }
    }
}
