using BibliotecaNET8.Application.DTOs.Autor;
using BibliotecaNET8.Application.DTOs.Categoria;
using BibliotecaNET8.Application.DTOs.Cliente;
using BibliotecaNET8.Application.DTOs.Libro;
using BibliotecaNET8.Application.DTOs.Prestamo;
using BibliotecaNET8.Application.Services.Impl;
using BibliotecaNET8.Application.Services.Interfaces;
using BibliotecaNET8.Application.Validators;
using BibliotecaNET8.DataAccess.Repositories;
using BibliotecaNET8.DataAccess.UnitOfWork.Impl;
using BibliotecaNET8.Domain.Repositories.Interfaces;
using BibliotecaNET8.Domain.UnitOfWork.Interfaces;
using FluentValidation;

namespace BibliotecaNET8.Web.Config;

public static class DIConfig
{
    public static void RegisterServicesAndRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAutorService, AutorService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<ILibroService, LibroService>();
        services.AddScoped<IPrestamoService, PrestamoService>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ILibroRepository, LibroRepository>();
        services.AddScoped<IPrestamoRepository, PrestamoRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void RegisterValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AutorDTO>, AutorValidator>();
        services.AddScoped<IValidator<CategoriaDTO>, CategoriaValidator>();
        services.AddScoped<IValidator<ClienteDTO>, ClienteValidator>();
        services.AddScoped<IValidator<LibroCreateDTO>, LibroCreateValidator>();
        services.AddScoped<IValidator<PrestamoCreateDTO>, PrestamoCreateValidator>();
    }
}
