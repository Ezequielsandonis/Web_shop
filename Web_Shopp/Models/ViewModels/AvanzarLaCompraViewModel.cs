namespace Web_Shop.Models.ViewModels
{
    public class AvanzarLaCompraViewModel
    {
        public CarritoViewModel Carrito { get; set; } = null!;
        public List<Direccion> Direcciones { get; set; } = null!;
        public int DireccionIdSeleccionada { get; set; }
    }
}
