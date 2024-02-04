namespace Web_Shop.Models.ViewModels
{
    public class CarritoViewModel
    {
        public List<CarritoItemViewModel> Items { get; set; } = new List<CarritoItemViewModel>();
        public decimal Total { get; set; }
    }
}
