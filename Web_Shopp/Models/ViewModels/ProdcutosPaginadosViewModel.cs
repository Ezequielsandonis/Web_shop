namespace Web_Shop.Models.ViewModels
{
    public class ProdcutosPaginadosViewModel
    {
        public List<Producto> Productos { get; set; } = null!;
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }

        public int ? CategoriaIdSeleccionada { get; set; }
        public string? Busqueda { get; set; }
        public bool MostrarMensajesSinResultados { get; set; }
        public string? NombreCategoriaSeleccionada { get; set; }
    }
}
