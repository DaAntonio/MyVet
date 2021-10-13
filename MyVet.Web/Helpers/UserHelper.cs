#region Using
using Microsoft.AspNetCore.Identity;
using MyVet.Web.Data.Entidades;
using MyVet.Web.Models;
using System.Threading.Tasks;
#endregion

namespace MyVet.Web.Helpers
{
    public class UserHelper : IUsuarioHelper
    {
        #region Variables 
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Usuario> _signInManager;
        #endregion

        #region Contructor
        public UserHelper(UserManager<Usuario> userManager,RoleManager<IdentityRole> roleManager,
            SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            
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

        public async Task<SignInResult> LoginAsync(LoginViewModel loginViewModel)
        {
            return await _signInManager.PasswordSignInAsync(
                loginViewModel.NobreUsuario,
                loginViewModel.Contracena,
                loginViewModel.Recordar,
                false
                );
        }

        public  async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public  async Task<bool> ElimianarUsuarioAsync(string email)
        {
            var usuario = await GetUsuarioByEmailAsync(email);
            if (usuario == null)
            {
                return true;
            }
            var eliminar = await _userManager.DeleteAsync(usuario);
            return eliminar.Succeeded;

        }

        public async Task<IdentityResult> EditarUsuarioAsync(Usuario usuario)
        {
            return await _userManager.UpdateAsync(usuario);   
        }

        public async Task<SignInResult> ValidaPasswordAsync(Usuario usuario, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(
                usuario,
                password,
                false);
        }

        #endregion
    }
}
