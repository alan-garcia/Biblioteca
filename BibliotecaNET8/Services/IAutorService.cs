using BibliotecaNET8.Models;
using BibliotecaNET8.ViewModels;
using System.Linq.Expressions;

namespace BibliotecaNET8.Services;

public interface IAutorService
{
    public Task<IQueryable<Autor>> GetAllAutores();
    public Task<PagedResult<Autor>> GetRecordsPagedResult(IQueryable<Autor> records, int pageNumber, int pageSize);
    public Task<Autor> GetAutorById(int? id);
    public Task AddAutor(Autor autor);
    public Task UpdateAutor(Autor autor);
    public Task<bool> DeleteAutor(int? id);
    public Task<bool> DeleteMultipleAutores(int[] ids);
    public IQueryable<Autor> SearchAutor(Expression<Func<Autor, bool>> predicate);
    public IQueryable<Autor> SearchAutor(IQueryable<Autor> queryType, Expression<Func<Autor, bool>> predicate);
}
