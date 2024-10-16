using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.DataAccess.Context;
using BibliotecaNET8.Domain.Repositories.Interfaces;
using BibliotecaNET8.Domain;

namespace BibliotecaNET8.DataAccess.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
{
    public readonly AppDbContext _context;
    public DbSet<T> Entity => _context.Set<T>();

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<T> GetAll() => Entity.AsNoTracking();

    public async Task<PagedResult<T>> GetRecordsPagedResult<T>(IQueryable<T> records, int pageNumber, int pageSize)
        where T : class
    {
        int totalItems = await records.CountAsync();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Definir cuántas páginas visibles mostrar
        int maxPagesToShow = 5;

        // Calcular el rango de páginas a mostrar
        int startPage = Math.Max(1, pageNumber - maxPagesToShow / 2);
        int endPage = Math.Min(totalPages, startPage + maxPagesToShow - 1);

        // Si hay pocas páginas al principio, ajusta el rango
        if (endPage - startPage < maxPagesToShow - 1)
        {
            startPage = Math.Max(1, endPage - maxPagesToShow + 1);
        }

        List<T> recordsPaged = await records
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            StartPage = startPage,
            EndPage = endPage,
            Items = recordsPaged
        };
    }

    public async Task<T?> GetById(int? id)
    {
        return await Entity.AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public void Add(T entity) => Entity.Add(entity);

    public void Update(T entity) => Entity.Update(entity);

    public async Task<bool> Delete(int? id)
    {
        var entity = await Entity.FindAsync(id);
        if (entity != null)
        {
            Entity.Remove(entity);
            return true;
        }

        return false;
    }

    public bool DeleteMultiple(int[] ids)
    {
        if (ids.Length > 0)
        {
            var idsSelected = Entity.Where(entity => ids.Contains(entity.Id));
            _context.Set<T>().RemoveRange(idsSelected);
            return true;
        }

        return false;
    }

    public IQueryable<T> Search(Expression<Func<T, bool>> predicate)
    {
        return predicate != null ? Entity.AsNoTracking().Where(predicate) : Entity;
    }

    public IQueryable<T> Search(IQueryable<T> queryType, Expression<Func<T, bool>> predicate)
    {
        return predicate != null ? queryType.AsNoTracking().Where(predicate) : queryType;
    }
}
