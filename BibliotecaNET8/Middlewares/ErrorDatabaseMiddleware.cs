using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaNET8.Web.Middlewares
{
    /// <summary>
    ///     Middleware personalizado para capturar los errores de conexión de Base de Datos en cualquier lugar de la aplicación
    /// </summary>
    public class ErrorDatabaseMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorDatabaseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException)
            {
                // Maneja el error de conexión a la base de datos
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Error de conexión a la base de datos.");
            }
        }
    }
}
