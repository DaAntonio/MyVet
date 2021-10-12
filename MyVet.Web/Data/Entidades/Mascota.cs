using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyVet.Web.Data.Entidades
{
    public class Mascota
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "El camopo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Nombre { get; set; }

        [Display(Name = "Foto")]
        public string UrlImagen { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Rasa { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        public string Comentarios { get; set; }

        public string ImagenFullPath => string.IsNullOrEmpty(UrlImagen)
            ? null
            : $"https://TBD.azurewebsites.net{UrlImagen.Substring(1)}";


        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaLocal => FechaNacimiento.ToLocalTime();

        public TipoMascota TipoMascota { get; set; }

        public Cliente Cliente { get; set; }
        public ICollection<HistorialMedico> HistorialMedicos { get; set; }
        public ICollection<Agenda> Agendas { get; set; }
    }
}
