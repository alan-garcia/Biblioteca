using BibliotecaNET8.Application.Services.Interfaces;
using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.Domain.Repositories.Interfaces;
using BibliotecaNET8.Domain.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace BibliotecaNET8.Application.Services.Impl;

public class LibroService : ILibroService
{
    private readonly ILibroRepository _libroRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LibroService(ILibroRepository libroRepository, IUnitOfWork unitOfWork)
    {
        _libroRepository = libroRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IQueryable<Libro>> GetAllLibros() => await _libroRepository.GetAll();

    public async Task<PagedResult<Libro>> GetRecordsPagedResult(IQueryable<Libro> records, int pageNumber, int pageSize)
    {
        return await _libroRepository.GetRecordsPagedResult(records, pageNumber, pageSize);
    }

    public async Task<Libro> GetLibroById(int? id)
    {
        return await _libroRepository.GetById(id)
            ?? throw new Exception("Libro no encontrado");
    }

    public async Task AddLibro(Libro libro)
    {
        if (libro == null)
        {
            throw new Exception("No se pudo añadir al libro");
        }

        await _libroRepository.Add(libro);
    }

    public async Task UpdateLibro(Libro libro)
    {
        if (libro == null)
        {
            throw new Exception("No se pudo actualizar el libro");
        }

        await _libroRepository.Update(libro);
    }

    public async Task<bool> DeleteLibro(int? id)
    {
        if (id == null)
        {
            throw new Exception("No se pudo borrar el libro");
        }

        return await _libroRepository.Delete(id);
    }

    public async Task<bool> DeleteMultipleLibros(int[] ids)
    {
        if (ids == null || ids.Length == 0)
        {
            throw new Exception("No se pudo borrar múltiples libros");
        }

        return await _libroRepository.DeleteMultiple(ids);
    }

    public IQueryable<Libro> SearchLibro(Expression<Func<Libro, bool>> predicate)
    {
        try
        {
            return _libroRepository.Search(predicate);
        }
        catch (Exception)
        {
            throw new Exception("Error en el filtro de búsqueda de libros");
        }
    }

    public IQueryable<Libro> SearchLibro(IQueryable<Libro> queryType, Expression<Func<Libro, bool>> predicate)
    {
        try
        {
            return _libroRepository.Search(queryType, predicate);
        }
        catch (Exception)
        {
            throw new Exception("Error en el filtro de búsqueda de libros");
        }
    }

    public IQueryable<Libro> GetLibrosConAutoresCategorias() => _libroRepository.GetLibrosConAutoresCategorias();

    public async Task<(Libro?, string?)> SetBinaryImage(IFormFile? Imagen, Libro? libro, string? ImagenActual = null)
    {
        if (Imagen?.Length > 0)
        {
            if (Imagen.Length > 2097152) // 2MB en bytes
            {
                return (null, "La imagen debe ser menor a 2 MB.");
            }

            await using var memoryStream = new MemoryStream();
            await Imagen.CopyToAsync(memoryStream);
            libro.Imagen = memoryStream?.ToArray() ?? [];
        }
        else if (!string.IsNullOrEmpty(ImagenActual))
        {
            libro.Imagen = Convert.FromBase64String(ImagenActual);
        }

        return (libro, null);
    }
}
