using BibliotecaNET8.Models;

namespace BibliotecaNET8.Repositories;

public interface ILibroRepository : IRepository<Libro>
{
    public IQueryable<Libro> GetLibrosConAutoresCategorias();
}
