using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Models
{
    public class AddUsuarioViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} carcteres.")]
        [EmailAddress]
        public string Correo { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener mas de {1} carcteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Documento { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carcteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carcteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Apellido { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} carcteres.")]
        public string Direccion { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} carcteres.")]
        public string NTelefono { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo  {0} debe contener {2} y {1} caractere.")]
        public string Password { get; set; }

        [Display(Name = "Password Confirm")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo  {0} debe contener {2} y {1} caractere.")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }


    }
}
