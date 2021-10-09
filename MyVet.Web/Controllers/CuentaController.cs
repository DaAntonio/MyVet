#region Using
using Microsoft.AspNetCore.Mvc;
using MyVet.Web.Helpers;
using MyVet.Web.Models;
using System.Linq;
using System.Threading.Tasks;
#endregion



namespace MyVet.Web.Controllers
{
    #region Permisos
    #endregion

    public class CuentaController:Controller
    {
        #region Variables
        private readonly IUsuarioHelper _usuarioHelper;
        #endregion

        #region Constructor	
        public CuentaController(IUsuarioHelper usuarioHelper)
        {
            _usuarioHelper = usuarioHelper;

        }

        #endregion

        #region Metodos	
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _usuarioHelper.LoginAsync(loginViewModel);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Usuario o contaseña invalidas");
            }
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _usuarioHelper.LogoutAsync();

            return RedirectToAction("Index", "Home");
        }

        #endregion


    }
    
}
