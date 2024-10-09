using BibliotecaNET8.Application.Services.Interfaces;
using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.Domain.Repositories.Interfaces;
using BibliotecaNET8.Domain.UnitOfWork.Interfaces;
using System.Linq.Expressions;

namespace BibliotecaNET8.Application.Services.Impl;

public class ClienteService : IClienteService
{
    private readonly IGenericRepository<Cliente> _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClienteService(IGenericRepository<Cliente> clienteRepository, IUnitOfWork unitOfWork)
    {
        _clienteRepository = clienteRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    ///     Obtiene todos los clientes.
    /// </summary>
    /// <returns>Lista de clientes.</returns>
    public async Task<IQueryable<Cliente>> GetAllClientes() => await _clienteRepository.GetAll();

    /// <summary>
    ///     Obtiene la lista de clientes paginados, con su número de página, tamaño de paginación, total elementos,
    ///     total páginas, y los elementos de la lista.
    /// </summary>
    /// <param name="records">La lista de clientes.</param>
    /// <param name="pageNumber">Número de página.</param>
    /// <param name="pageSize">Cantidad de elementos mostrados por página.</param>
    /// <returns>Lista de clientes en formato paginado.</returns>
    public async Task<PagedResult<Cliente>> GetRecordsPagedResult(IQueryable<Cliente> records, int pageNumber, int pageSize)
    {
        return await _clienteRepository.GetRecordsPagedResult(records, pageNumber, pageSize);
    }

    /// <summary>
    ///     Obtiene 1 cliente mediante su ID.
    /// </summary>
    /// <param name="id">ID del cliente.</param>
    /// <returns>El cliente correspondiente a su ID.</returns>
    public async Task<Cliente> GetClienteById(int? id)
    {
        return await _clienteRepository.GetById(id)
            ?? throw new Exception("Cliente no encontrado");
    }

    /// <summary>
    ///     Inserta un cliente.
    /// </summary>
    /// <param name="cliente">El cliente.</param>
    public async Task AddCliente(Cliente cliente)
    {
        if (cliente == null)
        {
            throw new Exception("No se pudo añadir al cliente");
        }

        await _clienteRepository.Add(cliente);
    }

    /// <summary>
    ///     Actualiza un cliente.
    /// </summary>
    /// <param name="cliente">El cliente.</param>
    public async Task UpdateCliente(Cliente cliente)
    {
        if (cliente == null)
        {
            throw new Exception("No se pudo actualizar el cliente");
        }

        await _clienteRepository.Update(cliente);
    }

    /// <summary>
    ///     Elimina un cliente.
    /// </summary>
    /// <param name="id">ID del cliente.</param>
    /// <returns>'true' si el cliente se ha eliminado correctamente, 'false' en caso contrario.</returns>
    public async Task<bool> DeleteCliente(int? id)
    {
        if (id == null)
        {
            throw new Exception("No se pudo borrar el cliente");
        }

        return await _clienteRepository.Delete(id);
    }

    /// <summary>
    ///     Elimina 1 o más clientes.
    /// </summary>
    /// <param name="ids">Lista de IDs de los clientes. Los checkboxes seleccionados en la vista almacenan sus IDs.</param>
    /// <returns>'true' si los clientes seleccionados se han eliminado correctamente, 'false' en caso contrario.</returns>
    public async Task<bool> DeleteMultipleClientes(int[] ids)
    {
        if (ids == null || ids.Length == 0)
        {
            throw new Exception("No se pudo borrar múltiples clientes");
        }

        return await _clienteRepository.DeleteMultiple(ids);
    }

    /// <summary>
    ///     Realiza un filtro de búsqueda de clientes.
    /// </summary>
    /// <param name="predicate">Condiciones de los filtros de búsqueda a aplicar.</param>
    /// <returns>Lista de clientes que cumplen con el criterio de búsqueda indicado.</returns>
    public IQueryable<Cliente> SearchCliente(Expression<Func<Cliente, bool>> predicate)
    {
        try
        {
            return _clienteRepository.Search(predicate);
        }
        catch (Exception)
        {
            throw new Exception("Error en el filtro de búsqueda de clientes");
        }
    }

    /// <summary>
    ///     Realiza un filtro de búsqueda de clientes.
    /// </summary>
    /// <param name="queryType">El tipo de la consulta a realizar en la búsqueda. Se utiliza para mostrar el 
    ///     listado de clientes sin filtrado de búsqueda</param>
    /// <param name="predicate">Condiciones de los filtros de búsqueda a aplicar.</param>
    /// <returns>Lista de clientes que cumplen con el criterio de búsqueda indicado.</returns>
    public IQueryable<Cliente> SearchCliente(IQueryable<Cliente> queryType, Expression<Func<Cliente, bool>> predicate)
    {
        try
        {
            return _clienteRepository.Search(queryType, predicate);
        }
        catch (Exception)
        {
            throw new Exception("Error en el filtro de búsqueda de clientes");
        }
    }
}
