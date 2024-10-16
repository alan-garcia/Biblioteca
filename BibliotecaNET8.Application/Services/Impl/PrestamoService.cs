using BibliotecaNET8.Application.Services.Interfaces;
using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.Domain.Exceptions;
using BibliotecaNET8.Domain.Repositories.Interfaces;
using System.Linq.Expressions;

namespace BibliotecaNET8.Application.Services.Impl;

public class PrestamoService : IPrestamoService
{
    private readonly IPrestamoRepository _prestamoRepository;

    public PrestamoService(IPrestamoRepository prestamoRepository)
    {
        _prestamoRepository = prestamoRepository;
    }

    /// <summary>
    ///     Obtiene todos los préstamos.
    /// </summary>
    /// <returns>Lista de préstamos.</returns>
    public IQueryable<Prestamo> GetAllPrestamos() => _prestamoRepository.GetAll();

    /// <summary>
    ///     Obtiene la lista de préstamos paginados, con su número de página, tamaño de paginación, total elementos,
    ///     total páginas, y los elementos de la lista.
    /// </summary>
    /// <param name="records">La lista de préstamos.</param>
    /// <param name="pageNumber">Número de página.</param>
    /// <param name="pageSize">Cantidad de elementos mostrados por página.</param>
    /// <returns>Lista de préstamos en formato paginado.</returns>
    public async Task<PagedResult<Prestamo>> GetRecordsPagedResult(IQueryable<Prestamo> records, int pageNumber, int pageSize)
    {
        return await _prestamoRepository.GetRecordsPagedResult(records, pageNumber, pageSize);
    }

    /// <summary>
    ///     Obtiene 1 préstamo mediante su ID.
    /// </summary>
    /// <param name="id">ID del préstamo.</param>
    /// <returns>El préstamo correspondiente a su ID.</returns>
    public async Task<Prestamo?> GetPrestamoById(int? id)
    {
        return await _prestamoRepository.GetById(id);
    }

    /// <summary>
    ///     Inserta un préstamo.
    /// </summary>
    /// <param name="prestamo">El préstamo.</param>
    public void AddPrestamo(Prestamo prestamo) => _prestamoRepository.Add(prestamo);

    /// <summary>
    ///     Actualiza un préstamo.
    /// </summary>
    /// <param name="prestamo">El préstamo.</param>
    public void UpdatePrestamo(Prestamo prestamo) => _prestamoRepository.Update(prestamo);

    /// <summary>
    ///     Elimina un préstamo.
    /// </summary>
    /// <param name="id">ID del préstamo.</param>
    /// <returns>'true' si el préstamo se ha eliminado correctamente, 'false' en caso contrario.</returns>
    public async Task<bool> DeletePrestamo(int? id)
    {
        if (id == null)
        {
            throw new CRUDException("No se pudo borrar el préstamo");
        }

        return await _prestamoRepository.Delete(id);
    }

    /// <summary>
    ///     Elimina 1 o más préstamos.
    /// </summary>
    /// <param name="ids">Lista de IDs de los préstamos. Los checkboxes seleccionados en la vista almacenan sus IDs.</param>
    /// <returns>'true' si los préstamos seleccionados se han eliminado correctamente, 'false' en caso contrario.</returns>
    public bool DeleteMultiplePrestamos(int[] ids)
    {
        if (ids == null || ids.Length == 0)
        {
            throw new CRUDException("No se pudo borrar múltiples préstamos");
        }

        return _prestamoRepository.DeleteMultiple(ids);
    }

    /// <summary>
    ///     Realiza un filtro de búsqueda de libros.
    /// </summary>
    /// <param name="predicate">Condiciones de los filtros de búsqueda a aplicar.</param>
    /// <returns>Lista de libros que cumplen con el criterio de búsqueda indicado.</returns>
    public IQueryable<Prestamo> SearchPrestamo(Expression<Func<Prestamo, bool>> predicate)
    {
        try
        {
            return _prestamoRepository.Search(predicate);
        }
        catch (Exception)
        {
            throw new SearchException("Error en el filtro de búsqueda de préstamo");
        }
    }

    /// <summary>
    ///     Realiza un filtro de búsqueda de préstamos.
    /// </summary>
    /// <param name="queryType">El tipo de la consulta a realizar en la búsqueda. Se utiliza para mostrar el 
    ///     listado de préstamos sin filtrado de búsqueda</param>
    /// <param name="predicate">Condiciones de los filtros de búsqueda a aplicar.</param>
    /// <returns>Lista de préstamos que cumplen con el criterio de búsqueda indicado.</returns>
    public IQueryable<Prestamo> SearchPrestamo(IQueryable<Prestamo> queryType, Expression<Func<Prestamo, bool>> predicate)
    {
        try
        {
            return _prestamoRepository.Search(queryType, predicate);
        }
        catch (Exception)
        {
            throw new SearchException("Error en el filtro de búsqueda de préstamo");
        }
    }

    /// <summary>
    ///     Carga las relaciones "Libro" y "Cliente" de la entidad "Préstamo".
    /// </summary>
    /// <returns>Lista de préstamos con las relaciones "Libro" y "Cliente" cargadas, con esto podremos acceder a 
    /// sus propiedades desde la entidad "Préstamo".</returns>
    public IQueryable<Prestamo> GetPrestamosConLibrosClientes() => _prestamoRepository.GetPrestamosConLibrosClientes();
}
