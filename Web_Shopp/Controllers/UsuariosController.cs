using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_Shop.Data;
using Web_Shop.Models;

namespace Web_Shop.Controllers
{
    [Authorize(Policy = "RequiredAdminOrStaff")]
    public class UsuariosController : BaseController
    {
       


        public UsuariosController(ApplicationDbContext context) : base(context) { }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Usuarios.Include(u => u.Rol);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Nombre,Telefono,NombreUsuario,Contrasenia,Correo,Direccion,Ciudad,Provincia,CodigoPostal,RolId")] 
        Usuario usuario
            
            
        )
        {
            var rol = await _context.Roles
                .Where(d=>d.RolId == usuario.RolId)
                .FirstOrDefaultAsync();

            if (rol != null )
            {
                usuario.Rol = rol;

                usuario.Direcciones = new List<Direccion>
                {
                    new Direccion
                    {
                        Address = usuario.Direccion,
                        Ciudad = usuario.Ciudad,
                        Provincia = usuario.Provincia,
                        CodigoPostal = usuario.CodigoPostal,
                    }
                };

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nombre,Telefono,NombreUsuario,Contrasenia,Correo,Direccion,Ciudad,Provincia,CodigoPostal,RolId")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            var rol = await _context.Roles
              .Where(d => d.RolId == usuario.RolId)
              .FirstOrDefaultAsync();

            if (rol!=null)
            {
              

                var existingUser = await _context.Usuarios
                    .Include(u=>u.Direcciones)
                    .FirstOrDefaultAsync(u=>u.UsuarioId == id);

                if (existingUser != null)
                {
                    if(existingUser.Direcciones.Count > 0)
                    {
                     var direccion = existingUser.Direcciones.First();
                        direccion.Address = usuario.Direccion;
                        direccion.Ciudad = usuario.Ciudad;
                        direccion.Provincia = usuario.Provincia;
                        direccion.CodigoPostal = usuario.CodigoPostal;
                    }
                    else
                    {
                        existingUser.Direcciones = new List<Direccion>
                        {
                            new Direccion
                            {
                                Address = usuario.Direccion,
                                Ciudad = usuario.Ciudad,
                                Provincia = usuario.Provincia,
                                CodigoPostal= usuario.CodigoPostal,
                            }
                        };
                    }

                    existingUser.Rol = rol;
                    existingUser.RolId = usuario.RolId;
                    existingUser.Nombre = usuario.Nombre;
                    existingUser.Telefono = usuario.Telefono;
                    existingUser.NombreUsuario = usuario.NombreUsuario;
                    existingUser.Contrasenia = usuario.Contrasenia;
                    existingUser.Correo = usuario.Correo;

                    try
                    {
                        _context.Update(existingUser);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException) {

                        if (!UsuarioExists(usuario.UsuarioId))
                        {
                            return NotFound();
                        } else 
                        {
                            throw;
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
            
            
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.UsuarioId == id)).GetValueOrDefault();
        }
    }
}
