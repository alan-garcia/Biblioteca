using Microsoft.EntityFrameworkCore;
using BibliotecaNET8.Context;
using BibliotecaNET8.Models;
using BibliotecaNET8.ViewModels;
using System.Linq.Expressions;

namespace BibliotecaNET8.Repositories;

public class Repository<T> : IRepository<T> where T : class, BaseEntity
{
    public readonly AppDbContext _context;
    public DbSet<T> Entity => _context.Set<T>();

    public Repository(AppDbContext context) => _context = context;

    public async Task<IQueryable<T>> GetAll() => Entity.AsNoTracking();

    public async Task<PagedResult<T>> GetRecordsPagedResult<T>(IQueryable<T> records, int pageNumber, int pageSize)
        where T : class
    {
        int totalItems = await records.CountAsync();

        List<T> recordsPaged = await records
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
            Items = recordsPaged
        };
    }

    public async Task<T?> GetById(int? id)
    {
        return await Entity.AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task Add(T entity)
    {
        Entity.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        Entity.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Delete(int? id)
    {
        var entity = await Entity.FindAsync(id);
        if (entity != null)
        {
            Entity.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteMultiple(int[] ids)
    {
        if (ids.Length > 0)
        {
            var idsSelected = Entity.Where(entity => ids.Contains(entity.Id));
            _context.Set<T>().RemoveRange(idsSelected);
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public IQueryable<T> Search(Expression<Func<T, bool>> predicate)
    {
        return predicate != null ? Entity.AsNoTracking().Where(predicate) : Entity;
    }

    public IQueryable<T> Search<T>(IQueryable<T> queryType, Expression<Func<T, bool>> predicate)
        where T : class
    {
        return predicate != null ? queryType.AsNoTracking().Where(predicate) : queryType;
    }
}
