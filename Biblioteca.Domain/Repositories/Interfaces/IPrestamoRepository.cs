using BibliotecaNET8.Domain.Entities;

namespace BibliotecaNET8.Domain.Repositories.Interfaces;

public interface IPrestamoRepository : IGenericRepository<Prestamo>
{
    public IQueryable<Prestamo> GetPrestamosConLibrosClientes();
}
