#region Using
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVet.Web.Data;
using MyVet.Web.Data.Entidades;
using System.Collections.Generic;
#endregion

namespace MyVet.Web.Controllers.API
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TipoMascotasController : ControllerBase
    {
        #region Variables
        private readonly DataContext _context;
        #endregion

        #region Constructro
        public TipoMascotasController(DataContext context)
        {
            _context = context;
        }
        #endregion

        #region Metodos
        [HttpGet]
        public IEnumerable<TipoMascota> GetTipoMascotas()
        {
            return _context.TipoMascotas;
        }
        #endregion

    }
}