using Microsoft.EntityFrameworkCore;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.DataAccess.Context;
using BibliotecaNET8.Domain.Repositories.Interfaces;

namespace BibliotecaNET8.DataAccess.Repositories;

public class LibroRepository : GenericRepository<Libro>, ILibroRepository
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
