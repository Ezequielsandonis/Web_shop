using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Shop.Data;
using Web_Shop.Models;

namespace Web_Shop.Controllers
{
   
    public class PerfilController : BaseController
    {
        public PerfilController(ApplicationDbContext context) : base(context) { }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            
                return NotFound();

                 var usuario = await _context.Usuarios
                    .Include(u => u.Direcciones)
                    .FirstOrDefaultAsync(u=>u.UsuarioId==id);

                if (usuario == null)
                return NotFound();

                return View(usuario);
            

        }

        public IActionResult AgregarDireccion( int id)
        {
            ViewBag.id=id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDireccion(Direccion direccion, int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>u.UsuarioId == id);
                if (usuario != null)
                    direccion.Usuario = usuario;

                _context.Add(direccion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id });
            }
            catch (Exception e)
            {

               return View(direccion);
            }

        }
       
    }
}
