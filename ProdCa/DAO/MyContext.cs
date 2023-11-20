using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProdCa.Models;

namespace ProdCa.DAO
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<Producto> Products { get; set; }
        public DbSet<Categoria> Categories { get; set; }
        public DbSet<Asociacion> Associations { get; set; }
    }
}
