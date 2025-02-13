﻿using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using System.Linq.Expressions;

namespace BibliotecaNET8.Application.Services.Interfaces;

public interface IClienteService
{
    public IQueryable<Cliente> GetAllClientes();
    public Task<PagedResult<Cliente>> GetRecordsPagedResult(IQueryable<Cliente> records, int pageNumber, int pageSize);
    public Task<Cliente?> GetClienteById(int? id);
    public void AddCliente(Cliente cliente);
    public void UpdateCliente(Cliente cliente);
    public Task<bool> DeleteCliente(int? id);
    public bool DeleteMultipleClientes(int[] ids);
    public IQueryable<Cliente> SearchCliente(Expression<Func<Cliente, bool>> predicate);
    public IQueryable<Cliente> SearchCliente(IQueryable<Cliente> queryType, Expression<Func<Cliente, bool>> predicate);
}
