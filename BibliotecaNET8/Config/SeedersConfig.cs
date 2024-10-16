using BibliotecaNET8.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaNET8.Web.Config;

/// <summary>
///     Configuración del comportamiento de los Seeders (los datos a cargar en la Base de Datos).
/// </summary>
public static class SeedersConfig
{
    /// <summary>
    ///     Al iniciar la aplicación, aplicar todas las migraciones pendientes de la Base de Datos,
    ///     y agregar los datos de prueba de todos los Seeders en el AppDbContext.
    /// </summary>
    /// <param name="app">Configuración proveniente del contenedor de dependencias (Program.cs).</param>
    public static void InitSeeds(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();

        // Aplicar migraciones pendientes y agregar datos con HasData()
        context.Database.EnsureDeleted();
        context.Database.Migrate();
    }

    /// <summary>
    ///     Cuando se detiene la aplicación, procede a eliminar todos los datos de prueba
    ///     en todas las tablas de la Base de Datos.
    /// </summary>
    /// <param name="app">Configuración proveniente del contenedor de dependencias (Program.cs).</param>
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
