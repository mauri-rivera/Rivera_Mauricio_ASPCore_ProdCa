namespace ProdCa.Models
{
    public class MyViewModel
    {
        public Producto newProduct { get; set; }

        public Categoria newCategory { get; set; }

        public List<Producto> ListaProductos { get; set; }

        public List<Categoria> ListaCategorias { get; set; }

        public List<Asociacion> ListaCategoriaPorProducto { get; set; }

        public List<Asociacion> ListaProductoPorCategoria { get; set; }

        public IEnumerable<string> ListaCategoriaExcluida { get; set; }

        public IEnumerable<string> ListaProductoExcluida { get; set; }
    }
}
