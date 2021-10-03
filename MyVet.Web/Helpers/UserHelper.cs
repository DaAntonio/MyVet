#region Using
using Microsoft.AspNetCore.Identity;
using MyVet.Web.Data.Entidades;
using System.Threading.Tasks;
#endregion

namespace MyVet.Web.Helpers
{
    public class UserHelper : IUsuarioHelper
    {
        #region Variables 
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        #endregion

        #region Contructor
        public UserHelper(UserManager<Usuario> userManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #endregion

        #region Metodos

        public async Task<Usuario> GetUsuarioByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> AddUsuarioAsync(Usuario usuario, string password)
        {
            return await _userManager.CreateAsync(usuario, password);
        }

        public async Task CheckRolAsync(string rolNombre)
        {
            var validaRol = await _roleManager.RoleExistsAsync(rolNombre);
            if (!validaRol)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name= rolNombre
                });
            }

        }

        public async Task AddUsuarioToRolAsync(Usuario usuario, string rolNombre)
        {
            await _userManager.AddToRoleAsync(usuario, rolNombre);
        }

        public async Task<bool> IsUsuarioInRolAsync(Usuario usuario,string rolNombre)
        {
            return await _userManager.IsInRoleAsync(usuario, rolNombre);
        }

        #endregion
    }
}
