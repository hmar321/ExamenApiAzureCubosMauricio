using ExamenApiAzureCubosMauricio.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamenApiAzureCubosMauricio.Data
{
    public class CubosContext : DbContext
    {
        public CubosContext(DbContextOptions<CubosContext> options) : base(options)
        {
        }

        public DbSet<Cubo> Cubos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
    }
}
