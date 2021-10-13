
#region Using
using Microsoft.AspNetCore.Identity;
using MyVet.Web.Data.Entidades;
using MyVet.Web.Models;
using System.Threading.Tasks;
#endregion
namespace MyVet.Web.Helpers
{
    public interface IUsuarioHelper
    {
        #region Metodos
        Task<Usuario> GetUsuarioByEmailAsync(string email);
        Task<IdentityResult> AddUsuarioAsync(Usuario usuario, string password);
        Task CheckRolAsync(string rolNombre);
        Task AddUsuarioToRolAsync(Usuario usuario, string rolNombre);
        Task<bool> IsUsuarioInRolAsync(Usuario usuario, string rolNombre);
        Task<SignInResult> LoginAsync(LoginViewModel loginViewModel);
        Task LogoutAsync();
        Task<bool> ElimianarUsuarioAsync(string email);
        Task<IdentityResult> EditarUsuarioAsync(Usuario usuario);
        Task<SignInResult> ValidaPasswordAsync(Usuario usuario, string password);


        #endregion

    }
}
