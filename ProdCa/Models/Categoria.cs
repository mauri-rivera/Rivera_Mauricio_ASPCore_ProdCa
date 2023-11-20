using System.ComponentModel.DataAnnotations;

namespace ProdCa.Models
{
    public class Categoria
    {
        [Key]
        [Required]
        public int CategoriaId { get; set; }

        [Required]
        public string Nombre {  get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Asociacion> AsociacionPorCategoria { get; set; } = new List<Asociacion>();
    }
}
