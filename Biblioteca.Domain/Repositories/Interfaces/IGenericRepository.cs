using System.Linq.Expressions;

namespace BibliotecaNET8.Domain.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    public Task<IQueryable<T>> GetAll();
    public Task<PagedResult<T>> GetRecordsPagedResult<T>(IQueryable<T> records, int pageNumber, int pageSize) where T: class;
    public Task<T?> GetById(int? id);
    public Task Add(T entity);
    public Task Update(T entity);
    public Task<bool> Delete(int? id);
    public Task<bool> DeleteMultiple(int[] ids);
    public IQueryable<T> Search(Expression<Func<T, bool>> predicate);
    public IQueryable<T> Search<T>(IQueryable<T> queryType, Expression<Func<T, bool>> predicate) where T: class;
}
