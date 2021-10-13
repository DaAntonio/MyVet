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

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Mascotas)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            if (cliente.Mascotas.Count >0)
            {

                return RedirectToAction(nameof(Index));
            }
            await _usuarioHelper.ElimianarUsuarioAsync(cliente.Usuario.Email);
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

            var owner = await _context.Clientes.FindAsync(id.Value);
            if (owner == null)
            {
                return NotFound();
            }

            var model = new MascotaViewModel
            {
                FechaNacimiento = DateTime.Today,
                ClienteId = owner.Id,
                TipoMascotas = _combosHelper.GetComboTipoMascota()
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> AddMascota(MascotaViewModel modeloMascota)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (modeloMascota.ImagenFile != null)
                {
                    path = await _imagenHelper.UploadImageAsync(modeloMascota.ImagenFile);
                }

                var pet = await _converterHelper.OjMascotaAsync(modeloMascota, path, true);
                _context.Mascotas.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction($"Details/{modeloMascota.ClienteId}");
            }

            return View(modeloMascota);


        }

        public async Task<IActionResult> EditMascota(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Mascota mascota = await _context.Mascotas.
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
                string path = modeloMascota.UrlImagen;

                if (modeloMascota.ImagenFile != null)
                {
                    path = await _imagenHelper.UploadImageAsync(modeloMascota.ImagenFile);
                }

                Mascota mascota = await _converterHelper.OjMascotaAsync(modeloMascota, path, false);
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
            Mascota mascota = await _context.Mascotas
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

        public async Task<IActionResult> EliminarMascota(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas
                .Include(c => c.Cliente)
                .Include(c => c.HistorialMedicos)
                .FirstOrDefaultAsync(c => c.Id == id.Value);
            if (mascota == null)
            {
                return NotFound();
            }
            if (mascota.HistorialMedicos.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "No se puede borrar el registro, existen datos de esta mascota.");
                return RedirectToAction($"{nameof(Details)}/{mascota.Cliente.Id}");
            }

            _context.Mascotas.Remove(mascota);
            await _context.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{mascota.Cliente.Id}");
        }
        #endregion

        #region Metodos para Historias
        public async Task<IActionResult> AgregarHistoria(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Mascota pet = await _context.Mascotas.FindAsync(id.Value);
            if (pet == null)
            {
                return NotFound();
            }

            HistoriaViewModel model = new HistoriaViewModel
            {
                Fecha = DateTime.Now,
                MascotaId = pet.Id,
                TipoServicios = _combosHelper.GetComboTipoServicio(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarHistoria(HistoriaViewModel model)
        {
            if (ModelState.IsValid)
            {
                HistorialMedico history = await _converterHelper.OjHistorialMedicoAsync(model, true);
                _context.HistorialMedicos.Add(history);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsMascota)}/{model.MascotaId}");
            }
            model.TipoServicios = _combosHelper.GetComboTipoServicio();
            return View(model);
        }


        public async Task<IActionResult> EditarHistoria(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var history = await _context.HistorialMedicos
                .Include(h => h.Mascota)
                .Include(h => h.TipoServicio)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (history == null)
            {
                return NotFound();
            }
            return View(_converterHelper.OjHistoriaViewModel(history));
        }

        [HttpPost]
        public async Task<IActionResult> EditarHistoria(HistoriaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var history = await _converterHelper.OjHistorialMedicoAsync(model, false);
                _context.HistorialMedicos.Update(history);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsMascota)}/{model.MascotaId}");
            }
            model.TipoServicios = _combosHelper.GetComboTipoServicio();
            return View(model);
        }

        public async Task<IActionResult> EliminarHistoria(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historia = await _context.HistorialMedicos
                .Include(h => h.Mascota)
                .FirstOrDefaultAsync(h => h.Id == id.Value);
            if (historia == null)
            {
                return NotFound();
            }

            _context.HistorialMedicos.Remove(historia);
            await _context.SaveChangesAsync();
            return RedirectToAction($"{nameof(DetailsMascota)}/{historia.Mascota.Id}");
        }


        #endregion
    }
}
