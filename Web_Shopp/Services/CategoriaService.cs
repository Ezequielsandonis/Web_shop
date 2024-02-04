using Microsoft.EntityFrameworkCore;
using Web_Shop.Data;
using Web_Shop.Models;

namespace Web_Shop.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ApplicationDbContext _context;

        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Categoria>> GetCategorias()
        {
           return await _context.Categorias.ToListAsync();
        }
    }
}
