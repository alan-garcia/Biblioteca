using BibliotecaNET8.Models;
using BibliotecaNET8.Repositories;
using BibliotecaNET8.ViewModels;
using System.Linq.Expressions;

namespace BibliotecaNET8.Services;

public class CategoriaService : ICategoriaService
{
    private readonly IRepository<Categoria> _categoriaRepository;

    public CategoriaService(IRepository<Categoria> categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<IQueryable<Categoria>> GetAllCategorias() => await _categoriaRepository.GetAll();

    public async Task<PagedResult<Categoria>> GetRecordsPagedResult(IQueryable<Categoria> records, int pageNumber, int pageSize)
    {
        return await _categoriaRepository.GetRecordsPagedResult(records, pageNumber, pageSize);
    }

    public async Task<Categoria> GetCategoriaById(int? id)
    {
        return await _categoriaRepository.GetById(id)
            ?? throw new Exception("Categoria no encontrada");
    }

    public async Task AddCategoria(Categoria categoria)
    {
        if (categoria == null)
        {
            throw new Exception("No se pudo añadir la categoría");
        }

        await _categoriaRepository.Add(categoria);
    }

    public async Task UpdateCategoria(Categoria categoria)
    {
        if (categoria == null)
        {
            throw new Exception("No se pudo actualizar la categoría");
        }

        await _categoriaRepository.Update(categoria);
    }

    public async Task<bool> DeleteCategoria(int? id)
    {
        if (id == null)
        {
            throw new Exception("No se pudo borrar la categoría");
        }

        return await _categoriaRepository.Delete(id);
    }

    public async Task<bool> DeleteMultipleCategorias(int[] ids)
    {
        if (ids == null || ids.Length == 0)
        {
            throw new Exception("No se pudo borrar múltiples categorías");
        }

        return await _categoriaRepository.DeleteMultiple(ids);
    }

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
