using Microsoft.EntityFrameworkCore;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.Domain.Repositories.Interfaces;
using BibliotecaNET8.DataAccess.Context;

namespace BibliotecaNET8.DataAccess.Repositories;

public class PrestamoRepository : GenericRepository<Prestamo>, IPrestamoRepository
{
    public PrestamoRepository(AppDbContext context) : base(context) { }

    public IQueryable<Prestamo> GetPrestamosConLibrosClientes()
    {
        return _context.Prestamo
            .Include(prestamo => prestamo.Libro)
            .Include(prestamo => prestamo.Cliente);
    }
}
