using BibliotecaNET8.Repositories;
using BibliotecaNET8.Services;
using BibliotecaNET8.Validators;
using BibliotecaNET8.ViewModels.Autor;
using BibliotecaNET8.ViewModels.Categoria;
using BibliotecaNET8.ViewModels.Cliente;
using BibliotecaNET8.ViewModels.Libro;
using BibliotecaNET8.ViewModels.Prestamo;
using FluentValidation;

namespace BibliotecaNET8.Config;

public static class DIConfig
{
    public static void RegisterServicesAndRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IAutorService, AutorService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<ILibroService, LibroService>();
        services.AddScoped<IPrestamoService, PrestamoService>();
        services.AddScoped<ILibroRepository, LibroRepository>();
        services.AddScoped<IPrestamoRepository, PrestamoRepository>();
    }

    public static void RegisterValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AutorVM>, AutorValidator>();
        services.AddScoped<IValidator<CategoriaVM>, CategoriaValidator>();
        services.AddScoped<IValidator<ClienteVM>, ClienteValidator>();
        services.AddScoped<IValidator<LibroCreateVM>, LibroCreateValidator>();
        services.AddScoped<IValidator<PrestamoCreateVM>, PrestamoCreateValidator>();
    }
}
