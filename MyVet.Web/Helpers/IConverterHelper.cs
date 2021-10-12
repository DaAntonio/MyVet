using MyVet.Web.Data.Entidades;
using MyVet.Web.Models;
using System.Threading.Tasks;

namespace MyVet.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<Mascota> OjMascotaAsync(MascotaViewModel modelo, string path, bool nuevaMascota);

        MascotaViewModel OjMascotaViewModel(Mascota mascota);
    }
}