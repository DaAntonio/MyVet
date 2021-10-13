using System;
using System.Collections.Generic;
using System.Text;

namespace MyVet.Common.Models
{
 public class HistoriaMedicaResponse
    {
        public int Id { get; set; }
        public string TipoServicio { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentarios { get; set; }

    }
}
