using Microsoft.AspNetCore.Identity;
using MyVet.Web.Data.Entidades;
using System.Threading.Tasks;

namespace MyVet.Web.Helpers
{
    public interface IUsuarioHelper
    {
        Task<Usuario> GetUsuarioByEmailAsync(string email);
        Task<IdentityResult> AddUsuarioAsync(Usuario usuario, string password);
        Task CheckRolAsync(string rolNombre);
        Task AddUsuarioToRolAsync(Usuario usuario, string rolNombre);
        Task<bool> IsUsuarioInRolAsync(Usuario usuario, string rolNombre);

    }
}
