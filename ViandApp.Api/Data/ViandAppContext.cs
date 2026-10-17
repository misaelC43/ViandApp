using Microsoft.EntityFrameworkCore;
using ViandApp.Api.Models;

namespace ViandApp.Api.Data;

public class ViandAppContext : DbContext
{
    public ViandAppContext(DbContextOptions<ViandAppContext> options) : base(options) { }

    // Cada DbSet se convierte en una tabla
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Vianda> Viandas => Set<Vianda>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<DetallePedido> DetallesPedido => Set<DetallePedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Datos iniciales: la base arranca con categorías y viandas cargadas
        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nombre = "Clásica" },
            new Categoria { Id = 2, Nombre = "Vegetariana" },
            new Categoria { Id = 3, Nombre = "Sin TACC" },
            new Categoria { Id = 4, Nombre = "Fitness" }
        );

        modelBuilder.Entity<Vianda>().HasData(
            new Vianda { Id = 1, Nombre = "Milanesa con puré", Descripcion = "Milanesa de carne con puré de papas", Precio = 5500, CategoriaId = 1 },
            new Vianda { Id = 2, Nombre = "Tarta de verduras", Descripcion = "Tarta de zapallito y choclo", Precio = 5000, CategoriaId = 2 },
            new Vianda { Id = 3, Nombre = "Pollo con arroz", Descripcion = "Pollo al horno con arroz", Precio = 5800, CategoriaId = 3 },
            new Vianda { Id = 4, Nombre = "Ensalada con pollo grillado", Descripcion = "Mix de hojas verdes con pollo", Precio = 6000, CategoriaId = 4 }
        );
    }
}