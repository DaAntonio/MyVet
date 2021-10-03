using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyVet.Web.Data.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Data
{
    //La class DataContext hereda de DbContext(:DbContext)
    public class DataContext:IdentityDbContext<Usuario>
    {
        public DataContext(DbContextOptions<DataContext>options): base(options)
        {

        }
    //Establecimiento de entidades mapeadas atrabes de propiedadres 
        public DbSet<Agenda>Agendas { get; set; }
        public DbSet<HistorialMedico> HistorialMedicos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<TipoMascota> TipoMascotas { get; set; }
        public DbSet<TipoServicio> TipoServicios { get; set; }
        public DbSet<Manager> Managers { get; set; }
        


    }
}
