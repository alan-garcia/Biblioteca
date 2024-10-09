using BibliotecaNET8.Domain.Entities;

namespace BibliotecaNET8.Domain.Repositories.Interfaces;

public interface ILibroRepository : IGenericRepository<Libro>
{
    public IQueryable<Libro> GetLibrosConAutoresCategorias();
}
