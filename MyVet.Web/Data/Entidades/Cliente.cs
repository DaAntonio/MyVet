using System.Collections.Generic;

namespace MyVet.Web.Data.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }

        public Usuario Usuario { get; set; }

        public ICollection<Mascota> Mascotas { get; set; }
        public ICollection<Agenda> Agendas { get; set; }
    }
}
