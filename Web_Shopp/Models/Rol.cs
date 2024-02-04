using System.ComponentModel.DataAnnotations;

namespace Web_Shop.Models
{
    public class Rol
    {
        [Key]
        public int RolId { get; set; }

        [Required(ErrorMessage = "Nombre es obligatorio")]
        [StringLength(30)]
        public string Nombre { get; set;} = null!;
    }
}
