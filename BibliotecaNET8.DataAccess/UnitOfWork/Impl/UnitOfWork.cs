using BibliotecaNET8.DataAccess.Context;
using BibliotecaNET8.Domain.UnitOfWork.Interfaces;

namespace BibliotecaNET8.DataAccess.UnitOfWork.Impl;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context) => _context = context;

    public async Task<int> Save() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
