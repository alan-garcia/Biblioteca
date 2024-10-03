using BibliotecaNET8.Models;
using BibliotecaNET8.Repositories;
using BibliotecaNET8.ViewModels;
using System.Linq.Expressions;

namespace BibliotecaNET8.Services;

public class PrestamoService : IPrestamoService
{
    private readonly IPrestamoRepository _prestamoRepository;

    public PrestamoService(IPrestamoRepository prestamoRepository)
    {
        _prestamoRepository = prestamoRepository;
    }

    public async Task<IQueryable<Prestamo>> GetAllPrestamos() => await _prestamoRepository.GetAll();

    public async Task<PagedResult<Prestamo>> GetRecordsPagedResult(IQueryable<Prestamo> records, int pageNumber, int pageSize)
    {
        return await _prestamoRepository.GetRecordsPagedResult(records, pageNumber, pageSize);
    }

    public async Task<Prestamo> GetPrestamoById(int? id)
    {
        return await _prestamoRepository.GetById(id)
            ?? throw new Exception("Préstamo no encontrado");
    }

    public async Task AddPrestamo(Prestamo prestamo)
    {
        if (prestamo == null)
        {
            throw new Exception("No se pudo añadir el préstamo");
        }

        await _prestamoRepository.Add(prestamo);
    }

    public async Task UpdatePrestamo(Prestamo prestamo)
    {
        if (prestamo == null)
        {
            throw new Exception("No se pudo actualizar el préstamo");
        }

        await _prestamoRepository.Update(prestamo);
    }

    public async Task<bool> DeletePrestamo(int? id)
    {
        if (id == null)
        {
            throw new Exception("No se pudo borrar el préstamo");
        }

        return await _prestamoRepository.Delete(id);
    }

    public async Task<bool> DeleteMultiplePrestamos(int[] ids)
    {
        if (ids == null || ids.Length == 0)
        {
            throw new Exception("No se pudo borrar múltiples préstamos");
        }

        return await _prestamoRepository.DeleteMultiple(ids);
    }

    public IQueryable<Prestamo> SearchPrestamo(Expression<Func<Prestamo, bool>> predicate)
    {
        try
        {
            return _prestamoRepository.Search(predicate);
        }
        catch (Exception)
        {
            throw new Exception("Error en el filtro de búsqueda de préstamo");
        }
    }

    public IQueryable<Prestamo> SearchPrestamo(IQueryable<Prestamo> queryType, Expression<Func<Prestamo, bool>> predicate)
    {
        try
        {
            return _prestamoRepository.Search(queryType, predicate);
        }
        catch (Exception)
        {
            throw new Exception("Error en el filtro de búsqueda de préstamo");
        }
    }

    public IQueryable<Prestamo> GetPrestamosConLibrosClientes() => _prestamoRepository.GetPrestamosConLibrosClientes();
}
