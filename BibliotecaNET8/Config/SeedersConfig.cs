using Microsoft.EntityFrameworkCore;
using BibliotecaNET8.Context;

namespace BibliotecaNET8.Config;

public static class SeedersConfig
{
    public static void InitSeeds(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();

        // Aplicar migraciones pendientes y agregar datos con HasData()
        context.Database.EnsureDeleted();
        context.Database.Migrate();
    }

    public static void ClearSeedsWhenStops(this WebApplication app)
    {
        var lifetime = app.Lifetime;
        lifetime.ApplicationStopping.Register(() =>
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();

            // Eliminar datos de las tablas
            context.Autores.RemoveRange(context.Autores);
            context.Categorias.RemoveRange(context.Categorias);
            context.Clientes.RemoveRange(context.Clientes);
            context.Libros.RemoveRange(context.Libros);
            context.Prestamo.RemoveRange(context.Prestamo);

            context.SaveChanges();
        });
    }
}
