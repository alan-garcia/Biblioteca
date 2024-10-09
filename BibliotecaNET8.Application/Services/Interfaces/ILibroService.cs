using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace BibliotecaNET8.Application.Services.Interfaces;

public interface ILibroService
{
    public Task<IQueryable<Libro>> GetAllLibros();
    public Task<PagedResult<Libro>> GetRecordsPagedResult(IQueryable<Libro> records, int pageNumber, int pageSize);
    public Task<Libro> GetLibroById(int? id);
    public Task AddLibro(Libro libro);
    public Task UpdateLibro(Libro libro);
    public Task<bool> DeleteLibro(int? id);
    public Task<bool> DeleteMultipleLibros(int[] ids);
    public IQueryable<Libro> SearchLibro(Expression<Func<Libro, bool>> predicate);
    public IQueryable<Libro> SearchLibro(IQueryable<Libro> queryType, Expression<Func<Libro, bool>> predicate);
    public IQueryable<Libro> GetLibrosConAutoresCategorias();
    public Task<(Libro?, string?)> SetBinaryImage(IFormFile? Imagen, Libro? libro, string? ImagenActual = null);
}
