using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyVet.Web.Data.Entidades;

namespace MyVet.Web.Models
{
    public class MascotaViewModel:Mascota
    {
        public int ClienteId { get; set; }

        [Required(ErrorMessage ="El camopo {0} es obligatorio")]
        [Display(Name ="Tipo Mascota")]
        [Range(1, int.MaxValue, ErrorMessage ="Selecciona una opcion validad")]
        public int TipoMascotaId { get; set; }

        [Display(Name ="Imagen")]
        public IFormFile ImagenFile { get; set; }

        public IEnumerable<SelectListItem> TipoMascotas { get; set; }
    }
}
