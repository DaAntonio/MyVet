#region Using
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVet.Web.Data;
using MyVet.Web.Data.Entidades;
using MyVet.Web.Helpers;
using MyVet.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace MyVet.Web.Controllers
{
    #region Permisos
    [Authorize(Roles = "Admin")]
    #endregion
    public class ClientesController : Controller
    {
        #region Variables
        private readonly DataContext _context;
        private readonly IUsuarioHelper _usuarioHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImagenHelper _imagenHelper;
        #endregion

        #region Constructor
        public ClientesController(DataContext context, IUsuarioHelper usuarioHelper,
            ICombosHelper combosHelper, IConverterHelper converterHelper, IImagenHelper imagenHelper)
        {
            _context = context;
            _usuarioHelper = usuarioHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _imagenHelper = imagenHelper;
        }
        #endregion

        #region Metodos para Cliente
        public IActionResult Index()
        {
            return View(_context.Clientes
                .Include(c => c.Usuario)
                .Include(c => c.Mascotas));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Cliente cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .Include(c => c.Mascotas)
                .ThenInclude(m => m.TipoMascota)
                .Include(c => c.Mascotas)
                .ThenInclude(m => m.HistorialMedicos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUsuarioViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario
                {
                    Direccion = modelo.Direccion,
                    Documento = modelo.Documento,
                    Email = modelo.Correo,
                    Nombre = modelo.Nombre,
                    Apellidos = modelo.Apellido,
                    PhoneNumber = modelo.NTelefono,
                    UserName = modelo.Correo
                };
                Microsoft.AspNetCore.Identity.IdentityResult respuesta = await _usuarioHelper.AddUsuarioAsync(usuario, modelo.Password);
                if (respuesta.Succeeded)
                {
                    Usuario usuariobd = await _usuarioHelper.GetUsuarioByEmailAsync(modelo.Correo);
                    await _usuarioHelper.AddUsuarioToRolAsync(usuariobd, "Customer");
                    Cliente cliente = new Cliente
                    {
                        Agendas = new List<Agenda>(),
                        Mascotas = new List<Mascota>(),
                        Usuario = usuariobd
                    };
                    _context.Clientes.Add(cliente);
                    try
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.ToString());
                        return View(modelo);
                    }
                }
                ModelState.AddModelError(string.Empty, respuesta.Errors.FirstOrDefault().Description);
            }
            return View(modelo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Cliente cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Cliente cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Cliente cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }

        #endregion

        #region Metodos para Mascota

        public async Task<IActionResult> AddMascota(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Cliente cliente = await _context.Clientes.FindAsync(id.Value);
            if (cliente == null)
            {
                return NotFound();
            }
            MascotaViewModel modelo = new MascotaViewModel
            {
                FechaNacimiento = DateTime.Today,
                ClienteId = cliente.Id,
                TipoMascotas = _combosHelper.GetComboTipoMascota()
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> AddMascota(MascotaViewModel modeloMascota)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;

                if (modeloMascota.ImagenFile != null)
                {
                    path = await _imagenHelper.UploadImageAsync(modeloMascota.ImagenFile);
                }

                Mascota mascota = await _converterHelper.OjMascotaAsync(modeloMascota, path, true);
                _context.Mascotas.Add(mascota);
                await _context.SaveChangesAsync();
                return RedirectToAction($"Details/{modeloMascota.ClienteId}");
            }
            modeloMascota.TipoMascotas = _combosHelper.GetComboTipoMascota();
            return View(modeloMascota);

        }
       
        public async Task<IActionResult> EditMascota(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mascota = await _context.Mascotas.
                Include(m => m.Cliente)
                .Include(m => m.TipoMascota)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (mascota == null)
            {
                return NotFound();
            }
            return View(_converterHelper.OjMascotaViewModel(mascota));
        }

        [HttpPost]
        public async Task<IActionResult> EditMascota(MascotaViewModel modeloMascota)
        {
            if (ModelState.IsValid)
            {
                var path = modeloMascota.UrlImagen;

                if (modeloMascota.ImagenFile != null)
                {
                    path = await _imagenHelper.UploadImageAsync(modeloMascota.ImagenFile);
                }

                var mascota = await _converterHelper.OjMascotaAsync(modeloMascota, path, false);
                _context.Mascotas.Update(mascota);
                await _context.SaveChangesAsync();
                return RedirectToAction($"Details/{modeloMascota.ClienteId}");
            }
            modeloMascota.TipoMascotas = _combosHelper.GetComboTipoMascota();
            return View(modeloMascota);

        }

        public async Task<IActionResult> DetailsMascota(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mascota = await _context.Mascotas
                .Include(m => m.Cliente)
                .ThenInclude(u => u.Usuario)
                .Include(m => m.HistorialMedicos)
                .ThenInclude(s => s.TipoServicio)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (mascota == null)
            {
                return NotFound();
            }
            return View(mascota);
        }
        
            
       #endregion
    }
}
