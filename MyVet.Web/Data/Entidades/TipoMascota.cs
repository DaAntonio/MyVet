using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Data.Entidades
{
    public class TipoMascota
    {
        public int Id { get; set; }

        [Display(Name = "Tipo Mascota")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required (ErrorMessage ="El campo {0} es obligatorio")]
        public String Valor{ get; set; }

        public ICollection<Mascota> Mascotas { get; set; }
    }
}
