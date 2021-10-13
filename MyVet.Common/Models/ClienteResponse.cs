using System;
using System.Collections.Generic;
using System.Text;

namespace MyVet.Common.Models
{
    public class ClienteResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Documento { get; set; }
        public string Direccion { get; set; }
        public string NTelefono { get; set; }
        public string Email { get; set; }
        public ICollection<MascotaResponse> Mascotas { get; set; }

    }
}
