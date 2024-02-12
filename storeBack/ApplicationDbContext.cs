using Microsoft.EntityFrameworkCore;
using storeBack.Models;

namespace storeBack
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<ArticuloCliente> ArticuloCliente { get; set; }
        public DbSet<ArticuloTienda> ArticuloTienda {  get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Tienda> Tienda { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    }
}
