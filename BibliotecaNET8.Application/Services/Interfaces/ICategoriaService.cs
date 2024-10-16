using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using System.Linq.Expressions;

namespace BibliotecaNET8.Application.Services.Interfaces;

public interface ICategoriaService
{
    public IQueryable<Categoria> GetAllCategorias();
    public Task<PagedResult<Categoria>> GetRecordsPagedResult(IQueryable<Categoria> records, int pageNumber, int pageSize);
    public Task<Categoria?> GetCategoriaById(int? id);
    public void AddCategoria(Categoria categoria);
    public void UpdateCategoria(Categoria categoria);
    public Task<bool> DeleteCategoria(int? id);
    public bool DeleteMultipleCategorias(int[] ids);
    public IQueryable<Categoria> SearchCategoria(Expression<Func<Categoria, bool>> predicate);
    public IQueryable<Categoria> SearchCategoria(IQueryable<Categoria> queryType, Expression<Func<Categoria, bool>> predicate);
}
