using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyVet.Web.Data.Entidades;

namespace MyVet.Web.Models
{
    public class HistoriaViewModel:HistorialMedico
    {
        public int MascotaId { get; set; }

        [Display(Name = "Servicio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona una opcion validad")]
        public int TipoServicioId { get; set; }

        public IEnumerable<SelectListItem> TipoServicios { get; set; }
    }
}
