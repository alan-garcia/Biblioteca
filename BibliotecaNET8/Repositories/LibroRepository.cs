using Microsoft.EntityFrameworkCore;
using BibliotecaNET8.Context;
using BibliotecaNET8.Models;

namespace BibliotecaNET8.Repositories;

public class LibroRepository : Repository<Libro>, ILibroRepository
{
    public LibroRepository(AppDbContext context) : base(context) { }

    public IQueryable<Libro> GetLibrosConAutoresCategorias()
    {
        return _context.Libros
            .Include(libro => libro.Autor)
            .Include(libro => libro.Categoria)
            .AsQueryable();
    }
}
