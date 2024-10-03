using BibliotecaNET8.Models;

namespace BibliotecaNET8.Repositories;

public interface IPrestamoRepository : IRepository<Prestamo>
{
    public IQueryable<Prestamo> GetPrestamosConLibrosClientes();
}
