using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Models
{
    public class EditUsuarioViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener mas de {1} carcteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Documento { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carcteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carcteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Apellido { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} carcteres.")]
        public string Direccion { get; set; }

        [Display(Name = "N.Telefono")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carcteres.")]
        public string NTelefono { get; set; }
    }
}
