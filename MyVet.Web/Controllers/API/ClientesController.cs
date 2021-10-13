#region  Using
using Microsoft.AspNetCore.Mvc;
using MyVet.Web.Data;
using System.Threading.Tasks;
using MyVet.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
#endregion

namespace MyVet.Web.Controllers.API
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientesController : ControllerBase
    {
        #region Variables
        private readonly DataContext _dataContext;
        #endregion

        #region Constructro
        public ClientesController(DataContext dataContext)
        {
            _dataContext = dataContext;

        }

        #endregion

        #region Metodos
        
        [HttpPost]
        [Route("ConsularClienteCorreo")]
        public async Task<IActionResult> ConsultarCliente(Email email)
        {
            if (! ModelState.IsValid)
            {
                return BadRequest();
            }

            var cliente = await _dataContext.Clientes
                .Include(c => c.Usuario)
                .Include(c => c.Mascotas)
                .ThenInclude(m => m.TipoMascota)
                .Include(c => c.Mascotas)
                .ThenInclude(p => p.HistorialMedicos)
                .ThenInclude(h => h.TipoServicio)
                .FirstOrDefaultAsync(c => c.Usuario.Email.ToLower() == email.Correo.ToLower());

            var response = new ClienteResponse
            {
                Nombre = cliente.Usuario.Nombre,
                Apellidos = cliente.Usuario.Apellidos,
                Direccion = cliente.Usuario.Direccion,
                Documento = cliente.Usuario.Documento,
                Email = cliente.Usuario.Email,
                NTelefono = cliente.Usuario.PhoneNumber,
                Mascotas = cliente.Mascotas.Select(p => new MascotaResponse
                {
                    FechaNacimiento = p.FechaNacimiento,
                    Id = p.Id,
                    ImagenUrl = p.ImagenFullPath,
                    Nombre = p.Nombre,
                    Rasa = p.Rasa,
                    Comentarios = p.Comentarios,
                    TipoMascota = p.TipoMascota.Valor,
                    HistoriaMedicas = p.HistorialMedicos.Select(h => new HistoriaMedicaResponse
                    {
                        Fecha = h.Fecha,
                        Descripcion = h.Descripcion,
                        Id = h.Id,
                        Comentarios = h.Comentarios,
                        TipoServicio = h.TipoServicio.Valor
                    }).ToList()
                }).ToList()
            };

            return Ok(response);

        }

        #endregion
    }
}
