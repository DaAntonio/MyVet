using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Models
{
    public class LoginViewModel
    {
       [Required]
       [EmailAddress]
       public string NobreUsuario { get; set; }

       [Required(ErrorMessage ="El campo {0} es obligatorio")]
       [MinLength(6)]
       [Display(Name="Contraseña") ]
       public string Contracena { get; set; }
       public bool Recordar { get; set; }

    }
}
