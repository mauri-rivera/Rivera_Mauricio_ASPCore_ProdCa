using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace ProdCa.Models
{
    public class Producto
    {
        [Key]
        [Required]
        public int ProductoId { get; set; }

        [Required]
        public string Nombre { get; set;}

        [Required]
        public string Descripcion { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Asociacion> AsociacionPorProducto { get; set; } = new List<Asociacion>();
    }
}
