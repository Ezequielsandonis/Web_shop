using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Shop.Models
{
    public class Producto
    {

        [Key]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = " El campo Codigo es requerido ")]
        [StringLength(500)]
        public string Codigo { get; set; } = null!;

        [Required(ErrorMessage = " El campo Nombre es requerido ")]
        [StringLength(500)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = " El campo Modelo es requerido ")]
        [StringLength(500)]
        public string Modelo { get; set; } = null!;

        [Required(ErrorMessage = " El campo Descripcion es requerido ")]
        [StringLength(1000)]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = " El campo Precio es requerido ")]
        [Range(0, int.MaxValue)]
        public decimal Precio { get; set; }

        [Required]
        [StringLength(255)]
        public string Imagen { get; set; } = null!;

        [Required]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; } = null!;

        [Required(ErrorMessage = " El campo Stock es requerido ")]
        public int Stock { get; set; }

        [Required(ErrorMessage = " El campo Marca es requerido ")]
        [StringLength(50)]
        public string Marca { get; set; } = null!;

        [Required(ErrorMessage = " El campo Activo es requerido ")]
        public bool Activo { get; set; }

        public ICollection<Detalle_Pedido> DetallesPedido { get; set; } = null!;
    }
}
