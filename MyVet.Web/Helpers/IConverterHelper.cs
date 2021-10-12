using MyVet.Web.Data.Entidades;
using MyVet.Web.Models;
using System.Threading.Tasks;

namespace MyVet.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<Mascota> OMascotaAsync(MascotaViewModel modelo, string path, bool isNew);
    }
}