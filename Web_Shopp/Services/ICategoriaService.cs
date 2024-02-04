using Web_Shop.Models;

namespace Web_Shop.Services
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> GetCategorias();
    }
}
