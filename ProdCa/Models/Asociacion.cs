using System.ComponentModel.DataAnnotations;

namespace ProdCa.Models
{
    public class Asociacion
    {
        [Key]
        [Required]
        public int AsociacionId { get; set; }

        public int ProductoId { get; set; }

        public int CategoriaId { get; set; }

        public Producto? Producto { get; set; }

        public Categoria? Categoria { get; set; }
    }
}
