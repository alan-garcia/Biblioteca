using BibliotecaNET8.Application.Services.Interfaces;
using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.Domain.Repositories.Interfaces;
using BibliotecaNET8.Domain.UnitOfWork.Interfaces;
using System.Linq.Expressions;

namespace BibliotecaNET8.Application.Services.Impl;

public class AutorService : IAutorService
{
    private readonly IGenericRepository<Autor> _autorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AutorService(IGenericRepository<Autor> autorRepository, IUnitOfWork unitOfWork)
    {
        _autorRepository = autorRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    ///     Obtiene todos los autores.
    /// </summary>
    /// <returns>Lista de autores.</returns>
    public async Task<IQueryable<Autor>> GetAllAutores() => await _autorRepository.GetAll();

    /// <summary>
    ///     Obtiene la lista de autores paginados, con su número de página, tamaño de paginación, total elementos,
    ///     total páginas, y los elementos de la lista.
    /// </summary>
    /// <param name="records">La lista de autores.</param>
    /// <param name="pageNumber">Número de página.</param>
    /// <param name="pageSize">Cantidad de elementos mostrados por página.</param>
    /// <returns>Lista de autores en formato paginado.</returns>
    public async Task<PagedResult<Autor>> GetRecordsPagedResult(IQueryable<Autor> records, int pageNumber, int pageSize)
    {
        return await _autorRepository.GetRecordsPagedResult(records, pageNumber, pageSize);
    }

    /// <summary>
    ///     Obtiene 1 autor mediante su ID.
    /// </summary>
    /// <param name="id">ID del autor.</param>
    /// <returns>El autor correspondiente a su ID.</returns>
    public async Task<Autor> GetAutorById(int? id)
    {
        return await _autorRepository.GetById(id)
            ?? throw new Exception("Autor no encontrado");
    }

    /// <summary>
    ///     Inserta un autor.
    /// </summary>
    /// <param name="autor">El autor.</param>
    public async Task AddAutor(Autor autor)
    {
        if (autor == null)
        {
            throw new Exception("No se pudo añadir al autor");
        }

        await _autorRepository.Add(autor);
    }

    /// <summary>
    ///     Actualiza un autor.
    /// </summary>
    /// <param name="autor">El autor.</param>
    public async Task UpdateAutor(Autor autor)
    {
        if (autor == null)
        {
            throw new Exception("No se pudo actualizar el autor");
        }

        await _autorRepository.Update(autor);
    }

    /// <summary>
    ///     Elimina un autor.
    /// </summary>
    /// <param name="id">ID del autor.</param>
    /// <returns>'true' si el autor se ha eliminado correctamente, 'false' en caso contrario.</returns>
    public async Task<bool> DeleteAutor(int? id)
    {
        if (id == null)
        {
            throw new Exception("No se pudo borrar al autor");
        }

        return await _autorRepository.Delete(id);
    }

    /// <summary>
    ///     Elimina 1 o más autores.
    /// </summary>
    /// <param name="ids">Lista de IDs de los autores. Los checkboxes seleccionados en la vista almacenan sus IDs.</param>
    /// <returns>'true' si los autores seleccionados se han eliminado correctamente, 'false' en caso contrario.</returns>
    public async Task<bool> DeleteMultipleAutores(int[] ids)
    {
        if (ids == null || ids.Length == 0)
        {
            throw new Exception("No se pudo borrar múltiples autores");
        }

        return await _autorRepository.DeleteMultiple(ids);
    }

    /// <summary>
    ///     Realiza un filtro de búsqueda de autores.
    /// </summary>
    /// <param name="predicate">Condiciones de los filtros de búsqueda a aplicar.</param>
    /// <returns>Lista de autores que cumplen con el criterio de búsqueda indicado.</returns>
    public IQueryable<Autor> SearchAutor(Expression<Func<Autor, bool>> predicate)
    {
        try
        {
            return _autorRepository.Search(predicate);
        }
        catch (Exception)
        {
            throw new Exception("Error en el filtro de búsqueda de autores");
        }
    }

    /// <summary>
    ///     Realiza un filtro de búsqueda de autores.
    /// </summary>
    /// <param name="queryType">El tipo de la consulta a realizar en la búsqueda. Se utiliza para mostrar el 
    ///     listado de autores sin filtrado de búsqueda</param>
    /// <param name="predicate">Condiciones de los filtros de búsqueda a aplicar.</param>
    /// <returns>Lista de autores que cumplen con el criterio de búsqueda indicado.</returns>
    public IQueryable<Autor> SearchAutor(IQueryable<Autor> queryType, Expression<Func<Autor, bool>> predicate)
    {
        try
        {
            return _autorRepository.Search(queryType, predicate);
        }
        catch (Exception)
        {
            throw new Exception("Error en el filtro de búsqueda de autores");
        }
    }
}
