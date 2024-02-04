using System.ComponentModel.DataAnnotations;

namespace Web_Shop.Models
{
    public class Categoria
    {
        //Inicializar Productos como lista de tipo Producto para evitar errores 
        public Categoria()
        {
            Productos = new List<Producto>();
        }


        [Key]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = " El campo Nombre es requerido ")]
        [StringLength(255)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = " El campo Descripcion es requerido ")]
        [StringLength(1000)]
        public string Descripcion { get; set; } = null!;

        public ICollection<Producto> Productos { get; set; } = null!;
    }
}
