using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Shop.Models
{
    public class Usuario
    {
        //inicializar Pedidos como una lista de tipo pedido
        public Usuario()
        {
            Pedidos=new List<Pedido>();
        }


        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = " El campo Nombre es requerido ")]
        [StringLength(50)]
        public string Nombre { get; set; } = null!;


        [Required(ErrorMessage = " El campo Telefono es requerido ")]
        [StringLength(15)]
        public string Telefono { get; set; } = null!;


        [Required(ErrorMessage = " El campo NombreUsuario es requerido ")]
        [StringLength(50)]
        public string NombreUsuario { get; set; } = null!;


        [Required(ErrorMessage = " El campo Contrasenia es requerido ")]
        [StringLength(250)]
        public string Contrasenia { get; set; } = null!;

        [Required(ErrorMessage = " El campo Correo es requerido ")]
        [StringLength(250)]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = " El campo Direccion es requerido ")]
        [StringLength(100)]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = " El campo Ciudad es requerido ")]
         [StringLength(20)]
        public string Ciudad { get; set; } = null!;

        [Required(ErrorMessage = " El campo Provincia es requerido ")]
        [StringLength(20)]
        public string Provincia { get; set; } = null!;

        [Required(ErrorMessage = " El campo Codigo Postal es requerido ")]
        [StringLength(10)]
        public string CodigoPostal { get; set; } = null!;

        public int RolId { get; set; }
        [ForeignKey("RolId")]

        [Required(ErrorMessage = " El campo Rol es requerido ")]
        public Rol Rol { get; set; } = null!;

        public ICollection<Pedido> Pedidos { get; set; } 
        [InverseProperty("Usuario")]
        public ICollection<Direccion> Direcciones { get; set; }=null!;



    }
}
