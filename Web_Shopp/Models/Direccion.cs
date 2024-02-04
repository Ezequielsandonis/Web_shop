using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Shop.Models
{
    public class Direccion
    {
        [Key]
        public int DirerccionId { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = " El campo Ciudad es requerido ")]
        [StringLength(20)]
        public string Ciudad { get; set; } = null!;

        [Required(ErrorMessage = " El campo Provincia es requerido ")]
        [StringLength(20)]
        public string Provincia { get; set; } = null!;

        [Required(ErrorMessage = " El campo Codigo Postal es requerido ")]
        [StringLength(10)]
        public string CodigoPostal { get; set; } = null!;

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!;
    }
}