using Web_Shop.Models;
using Web_Shop.Models.ViewModels;

namespace Web_Shop.Services
{
    public interface IproductoService
    {
        Producto GetProducto(int id);

        Task<List<Producto>> GetProductosDestacados();

        Task<ProdcutosPaginadosViewModel> GetProductosPaginados(int? categoriaId, string? busqueda, int pagina, int productosPorPagina);
    }
}
