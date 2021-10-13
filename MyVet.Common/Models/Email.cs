using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyVet.Common.Models
{
   public class Email
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; }
    }
}
