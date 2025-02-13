﻿using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using System.Linq.Expressions;

namespace BibliotecaNET8.Application.Services.Interfaces;

public interface IPrestamoService
{
    public IQueryable<Prestamo> GetAllPrestamos();
    public Task<PagedResult<Prestamo>> GetRecordsPagedResult(IQueryable<Prestamo> records, int pageNumber, int pageSize);
    public Task<Prestamo?> GetPrestamoById(int? id);
    public void AddPrestamo(Prestamo prestamo);
    public void UpdatePrestamo(Prestamo prestamo);
    public Task<bool> DeletePrestamo(int? id);
    public bool DeleteMultiplePrestamos(int[] ids);
    public IQueryable<Prestamo> SearchPrestamo(Expression<Func<Prestamo, bool>> predicate);
    public IQueryable<Prestamo> SearchPrestamo(IQueryable<Prestamo> queryType, Expression<Func<Prestamo, bool>> predicate);
    public IQueryable<Prestamo> GetPrestamosConLibrosClientes();
}
