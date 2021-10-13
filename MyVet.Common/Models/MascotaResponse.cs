using System;
using System.Collections.Generic;
using System.Text;

namespace MyVet.Common.Models
{
   public class MascotaResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ImagenUrl { get; set; }
        public string Rasa { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Comentarios { get; set; }
        public string TipoMascota { get; set; }
        public ICollection<HistoriaMedicaResponse> HistoriaMedicas { get; set; }


    }
}
