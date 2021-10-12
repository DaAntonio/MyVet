
using System;
using System.ComponentModel.DataAnnotations;


namespace MyVet.Web.Data.Entidades
{
    public class HistorialMedico
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "El camopo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString ="{0:yyyy/MM/dd}", ApplyFormatInEditMode =true)]
        public DateTime Fecha { get; set; }
        public string Comentarios { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Display (Name ="Fecha")]
        public DateTime FechaLocal => Fecha.ToLocalTime();

        public TipoServicio TipoServicio { get; set; }
        public Mascota Mascota { get; set; }
    }
}
