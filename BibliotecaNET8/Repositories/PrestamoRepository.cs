using Microsoft.EntityFrameworkCore;
using BibliotecaNET8.Context;
using BibliotecaNET8.Models;

namespace BibliotecaNET8.Repositories;

public class PrestamoRepository : Repository<Prestamo>, IPrestamoRepository
{
    public PrestamoRepository(AppDbContext context) : base(context) { }

    public IQueryable<Prestamo> GetPrestamosConLibrosClientes()
    {
        return _context.Prestamo
            .Include(prestamo => prestamo.Libro)
            .Include(prestamo => prestamo.Cliente);
    }
}
