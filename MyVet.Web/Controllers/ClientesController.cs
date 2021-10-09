#region Using
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVet.Web.Data;
using MyVet.Web.Data.Entidades;
using MyVet.Web.Models;
using System.Linq;
using System.Threading.Tasks;
using MyVet.Web.Helpers;
using System.Collections.Generic;
using System;
#endregion

namespace MyVet.Web.Controllers
{
    #region Permisos
    [Authorize (Roles ="Admin")]
    #endregion
    public class ClientesController : Controller
    {
        #region Variables
        private readonly DataContext _context;
        private readonly IUsuarioHelper _usuarioHelper;
        #endregion

        #region Constructor
        public ClientesController(DataContext context,IUsuarioHelper usuarioHelper)
        {
            _context = context;
            _usuarioHelper = usuarioHelper;
        }
        #endregion

        #region Metodos
        public  IActionResult Index()
        {
            return View(_context.Clientes
                .Include(c=>c.Usuario)
                .Include(c => c.Mascotas));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .Include(c => c.Mascotas)
                .ThenInclude(m=>m.TipoMascota)
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
                var usuario = new Usuario
                {
                    Direccion = modelo.Direccion,
                    Documento = modelo.Documento,
                    Email = modelo.Correo,
                    Nombre = modelo.Nombre,
                    Apellidos = modelo.Apellido,
                    PhoneNumber = modelo.NTelefono,
                    UserName = modelo.Correo

                };
                var respuesta = await _usuarioHelper.AddUsuarioAsync(usuario, modelo.Password);
                if (respuesta.Succeeded)
                {
                    var usuariobd = await _usuarioHelper.GetUsuarioByEmailAsync(modelo.Correo);
                    await _usuarioHelper.AddUsuarioToRolAsync(usuariobd, "Customer");

                    var cliente = new Cliente
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

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
        #endregion
    }
}
