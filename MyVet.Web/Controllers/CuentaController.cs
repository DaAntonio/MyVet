#region Using
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyVet.Web.Helpers;
using MyVet.Web.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor	
        public CuentaController(IUsuarioHelper usuarioHelper,
            IConfiguration configuration)
        {
            _usuarioHelper = usuarioHelper;
            _configuration = configuration;
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
                ModelState.AddModelError(string.Empty, "Usuario o contaseña invalidos");
            }
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _usuarioHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CrearToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _usuarioHelper.GetUsuarioByEmailAsync(model.NobreUsuario);
                if (user != null)
                {
                    var result = await _usuarioHelper.ValidaPasswordAsync(
                        user,
                        model.Contracena);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }

        #endregion


    }

}
