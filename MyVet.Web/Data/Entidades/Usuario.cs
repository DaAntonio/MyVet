using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyVet.Web.Data.Entidades
{
    public class Usuario: IdentityUser
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener mas de {1} caractere")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caractere")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caractere")]
        public string Apellidos { get; set; }

        [MaxLength(100)]
        public string Direccion { get; set; }

        //Propiedad de lectura 
        [Display(Name = "Cliente")]
        public String NombreCompleto => $"{Nombre}{Apellidos}";


    }
}
