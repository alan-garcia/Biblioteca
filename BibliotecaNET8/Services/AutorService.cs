using BibliotecaNET8.Models;
using BibliotecaNET8.Repositories;
using BibliotecaNET8.ViewModels;
using System.Linq.Expressions;

namespace BibliotecaNET8.Services;

public class AutorService : IAutorService
{
    private readonly IRepository<Autor> _autorRepository;

    public AutorService(IRepository<Autor> autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public async Task<IQueryable<Autor>> GetAllAutores() => await _autorRepository.GetAll();

    public async Task<PagedResult<Autor>> GetRecordsPagedResult(IQueryable<Autor> records, int pageNumber, int pageSize)
    {
        return await _autorRepository.GetRecordsPagedResult(records, pageNumber, pageSize);
    }

    public async Task<Autor> GetAutorById(int? id)
    {
        return await _autorRepository.GetById(id)
            ?? throw new Exception("Autor no encontrado");
    }

    public async Task AddAutor(Autor autor)
    {
        if (autor == null)
        {
            throw new Exception("No se pudo añadir al autor");
        }

        await _autorRepository.Add(autor);
    }

    public async Task UpdateAutor(Autor autor)
    {
        if (autor == null)
        {
            throw new Exception("No se pudo actualizar el autor");
        }

        await _autorRepository.Update(autor);
    }

    public async Task<bool> DeleteAutor(int? id)
    {
        if (id == null)
        {
            throw new Exception("No se pudo borrar al autor");
        }

        return await _autorRepository.Delete(id);
    }

    public async Task<bool> DeleteMultipleAutores(int[] ids)
    {
        if (ids == null || ids.Length == 0)
        {
            throw new Exception("No se pudo borrar múltiples autores");
        }

        return await _autorRepository.DeleteMultiple(ids);
    }

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
