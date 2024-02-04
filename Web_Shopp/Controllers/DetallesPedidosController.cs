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
    public class DetallesPedidosController :BaseController
    {
      

        public DetallesPedidosController(ApplicationDbContext context) : base(context) { }


        // GET: DetallesPedidos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Detalle_Pedidos.Include(d => d.Pedido).Include(d => d.Producto);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DetallesPedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Detalle_Pedidos == null)
            {
                return NotFound();
            }

            var detalle_Pedido = await _context.Detalle_Pedidos
                .Include(d => d.Pedido)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.DetallePedidoId == id);
            if (detalle_Pedido == null)
            {
                return NotFound();
            }

            return View(detalle_Pedido);
        }

        // GET: DetallesPedidos/Create
        public IActionResult Create()
        {
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "Estado");
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "Codigo");
            return View();
        }

        // POST: DetallesPedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DetallePedidoId,PedidoId,ProductoId,Cantidad,precio")] Detalle_Pedido detalle_Pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalle_Pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "Estado", detalle_Pedido.PedidoId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "Codigo", detalle_Pedido.ProductoId);
            return View(detalle_Pedido);
        }

        // GET: DetallesPedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Detalle_Pedidos == null)
            {
                return NotFound();
            }

            var detalle_Pedido = await _context.Detalle_Pedidos.FindAsync(id);
            if (detalle_Pedido == null)
            {
                return NotFound();
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "Estado", detalle_Pedido.PedidoId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "Codigo", detalle_Pedido.ProductoId);
            return View(detalle_Pedido);
        }

        // POST: DetallesPedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DetallePedidoId,PedidoId,ProductoId,Cantidad,precio")] Detalle_Pedido detalle_Pedido)
        {
            if (id != detalle_Pedido.DetallePedidoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalle_Pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Detalle_PedidoExists(detalle_Pedido.DetallePedidoId))
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
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "Estado", detalle_Pedido.PedidoId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "Codigo", detalle_Pedido.ProductoId);
            return View(detalle_Pedido);
        }

        // GET: DetallesPedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Detalle_Pedidos == null)
            {
                return NotFound();
            }

            var detalle_Pedido = await _context.Detalle_Pedidos
                .Include(d => d.Pedido)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.DetallePedidoId == id);
            if (detalle_Pedido == null)
            {
                return NotFound();
            }

            return View(detalle_Pedido);
        }

        // POST: DetallesPedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Detalle_Pedidos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Detalle_Pedidos'  is null.");
            }
            var detalle_Pedido = await _context.Detalle_Pedidos.FindAsync(id);
            if (detalle_Pedido != null)
            {
                _context.Detalle_Pedidos.Remove(detalle_Pedido);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Detalle_PedidoExists(int id)
        {
          return (_context.Detalle_Pedidos?.Any(e => e.DetallePedidoId == id)).GetValueOrDefault();
        }
    }
}
