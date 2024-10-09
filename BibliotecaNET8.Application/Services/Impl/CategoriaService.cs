using BibliotecaNET8.Application.Services.Interfaces;
using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.Domain.Repositories.Interfaces;
using BibliotecaNET8.Domain.UnitOfWork.Interfaces;
using System.Linq.Expressions;

namespace BibliotecaNET8.Application.Services.Impl;

public class CategoriaService : ICategoriaService
{
    private readonly IGenericRepository<Categoria> _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoriaService(IGenericRepository<Categoria> categoriaRepository, IUnitOfWork unitOfWork)
    {
        _categoriaRepository = categoriaRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    ///     Obtiene todas las categorías.
    /// </summary>
    /// <returns>Lista de categorías.</returns>
    public async Task<IQueryable<Categoria>> GetAllCategorias() => await _categoriaRepository.GetAll();

    /// <summary>
    ///     Obtiene la lista de categorías paginadas, con su número de página, tamaño de paginación, total elementos,
    ///     total páginas, y los elementos de la lista.
    /// </summary>
    /// <param name="records">La lista de categorías.</param>
    /// <param name="pageNumber">Número de página.</param>
    /// <param name="pageSize">Cantidad de elementos mostrados por página.</param>
    /// <returns>Lista de categorías en formato paginado.</returns>
    public async Task<PagedResult<Categoria>> GetRecordsPagedResult(IQueryable<Categoria> records, int pageNumber, int pageSize)
    {
        return await _categoriaRepository.GetRecordsPagedResult(records, pageNumber, pageSize);
    }

    /// <summary>
    ///     Obtiene 1 categoría mediante su ID.
    /// </summary>
    /// <param name="id">ID de la categoría.</param>
    /// <returns>La categoría correspondiente a su ID.</returns>
    public async Task<Categoria> GetCategoriaById(int? id)
    {
        return await _categoriaRepository.GetById(id)
            ?? throw new Exception("Categoria no encontrada");
    }

    /// <summary>
    ///     Inserta una categoría.
    /// </summary>
    /// <param name="categoria">La categoría.</param>
    public async Task AddCategoria(Categoria categoria)
    {
        if (categoria == null)
        {
            throw new Exception("No se pudo añadir la categoría");
        }

        await _categoriaRepository.Add(categoria);
    }

    /// <summary>
    ///     Actualiza una categoría.
    /// </summary>
    /// <param name="categoria">La categoría.</param>
    public async Task UpdateCategoria(Categoria categoria)
    {
        if (categoria == null)
        {
            throw new Exception("No se pudo actualizar la categoría");
        }

        await _categoriaRepository.Update(categoria);
    }

    /// <summary>
    ///     Elimina una categoría.
    /// </summary>
    /// <param name="id">ID de la categoría.</param>
    /// <returns>'true' si la categoría se ha eliminado correctamente, 'false' en caso contrario.</returns>
    public async Task<bool> DeleteCategoria(int? id)
    {
        if (id == null)
        {
            throw new Exception("No se pudo borrar la categoría");
        }

        return await _categoriaRepository.Delete(id);
    }

    /// <summary>
    ///     Elimina 1 o más categorías.
    /// </summary>
    /// <param name="ids">Lista de IDs de las categorías. Los checkboxes seleccionados en la vista almacenan sus IDs.</param>
    /// <returns>'true' si las categorías seleccionadas se han eliminado correctamente, 'false' en caso contrario.</returns>
    public async Task<bool> DeleteMultipleCategorias(int[] ids)
    {
        if (ids == null || ids.Length == 0)
        {
            throw new Exception("No se pudo borrar múltiples categorías");
        }

        return await _categoriaRepository.DeleteMultiple(ids);
    }

    /// <summary>
    ///     Realiza un filtro de búsqueda de categorías.
    /// </summary>
    /// <param name="predicate">Condiciones de los filtros de búsqueda a aplicar.</param>
    /// <returns>Lista de autores que cumplen con el criterio de búsqueda indicado.</returns>
    public IQueryable<Categoria> SearchCategoria(Expression<Func<Categoria, bool>> predicate)
    {
        try
        {
            return _categoriaRepository.Search(predicate);
        }
        catch (Exception)
        {
            throw new Exception("Error en el filtro de búsqueda de categorías");
        }
    }

    /// <summary>
    ///     Realiza un filtro de búsqueda de categorías.
    /// </summary>
    /// <param name="queryType">El tipo de la consulta a realizar en la búsqueda. Se utiliza para mostrar el 
    ///     listado de categorías sin filtrado de búsqueda</param>
    /// <param name="predicate">Condiciones de los filtros de búsqueda a aplicar.</param>
    /// <returns>Lista de categorías que cumplen con el criterio de búsqueda indicado.</returns>
    public IQueryable<Categoria> SearchCategoria(IQueryable<Categoria> queryType, Expression<Func<Categoria, bool>> predicate)
    {
        try
        {
            return _categoriaRepository.Search(queryType, predicate);
        }
        catch (Exception)
        {
            throw new Exception("Error en el filtro de búsqueda de categorías");
        }
    }
}
