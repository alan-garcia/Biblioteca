using Microsoft.EntityFrameworkCore;
using BibliotecaNET8.Models;
using BibliotecaNET8.Seeds;

namespace BibliotecaNET8.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

    public DbSet<Autor> Autores { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Libro> Libros { get; set; }
    public DbSet<Prestamo> Prestamo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AutorSeed());
        modelBuilder.ApplyConfiguration(new CategoriaSeed());
        modelBuilder.ApplyConfiguration(new ClienteSeed());
        modelBuilder.ApplyConfiguration(new LibroSeed());
        modelBuilder.ApplyConfiguration(new PrestamoSeed());
        base.OnModelCreating(modelBuilder);
    }
}
