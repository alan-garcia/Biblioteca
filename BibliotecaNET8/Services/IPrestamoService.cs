using BibliotecaNET8.Models;
using BibliotecaNET8.ViewModels;
using System.Linq.Expressions;

namespace BibliotecaNET8.Services;

public interface IPrestamoService
{
    public Task<IQueryable<Prestamo>> GetAllPrestamos();
    public Task<PagedResult<Prestamo>> GetRecordsPagedResult(IQueryable<Prestamo> records, int pageNumber, int pageSize);
    public Task<Prestamo> GetPrestamoById(int? id);
    public Task AddPrestamo(Prestamo prestamo);
    public Task UpdatePrestamo(Prestamo prestamo);
    public Task<bool> DeletePrestamo(int? id);
    public Task<bool> DeleteMultiplePrestamos(int[] ids);
    public IQueryable<Prestamo> SearchPrestamo(Expression<Func<Prestamo, bool>> predicate);
    public IQueryable<Prestamo> SearchPrestamo(IQueryable<Prestamo> queryType, Expression<Func<Prestamo, bool>> predicate);
    public IQueryable<Prestamo> GetPrestamosConLibrosClientes();
}
