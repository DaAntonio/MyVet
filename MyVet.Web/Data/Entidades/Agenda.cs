using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Data.Entidades
{
    public class Agenda
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.DateTime)]
        
        public DateTime Fecha{ get; set; }

        public int Comentarios { get; set; }

        public bool Disponibilidad { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd H:mm tt}")]
        public DateTime FechaLocal => Fecha.ToLocalTime();

        public Cliente Cliente { get; set; }
        public Mascota Mascota { get; set; }
    }
}
