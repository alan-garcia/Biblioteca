using BibliotecaNET8.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BibliotecaNET8.Infrastructure;

/// <summary>
///     Configuración de la cadena de conexión de la Base de Datos
/// </summary>
public static class DatabaseConfig
{
    public static IServiceCollection ConnectToDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MSSQLConnection"))
        );

        return services;
    }
}
