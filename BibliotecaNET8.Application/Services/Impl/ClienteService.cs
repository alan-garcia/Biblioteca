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

    public async Task<IQueryable<Cliente>> GetAllClientes() => await _clienteRepository.GetAll();

    public async Task<PagedResult<Cliente>> GetRecordsPagedResult(IQueryable<Cliente> records, int pageNumber, int pageSize)
    {
        return await _clienteRepository.GetRecordsPagedResult(records, pageNumber, pageSize);
    }

    public async Task<Cliente> GetClienteById(int? id)
    {
        return await _clienteRepository.GetById(id)
            ?? throw new Exception("Cliente no encontrado");
    }

    public async Task AddCliente(Cliente cliente)
    {
        if (cliente == null)
        {
            throw new Exception("No se pudo añadir al cliente");
        }

        await _clienteRepository.Add(cliente);
    }

    public async Task UpdateCliente(Cliente cliente)
    {
        if (cliente == null)
        {
            throw new Exception("No se pudo actualizar el cliente");
        }

        await _clienteRepository.Update(cliente);
    }

    public async Task<bool> DeleteCliente(int? id)
    {
        if (id == null)
        {
            throw new Exception("No se pudo borrar el cliente");
        }

        return await _clienteRepository.Delete(id);
    }

    public async Task<bool> DeleteMultipleClientes(int[] ids)
    {
        if (ids == null || ids.Length == 0)
        {
            throw new Exception("No se pudo borrar múltiples clientes");
        }

        return await _clienteRepository.DeleteMultiple(ids);
    }

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
