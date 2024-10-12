using BibliotecaNET8.Application.Services.Interfaces;
using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.Domain.Exceptions;
using BibliotecaNET8.Domain.Repositories.Interfaces;
using BibliotecaNET8.Domain.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace BibliotecaNET8.Application.Services.Impl;

public class LibroService : ILibroService
{
    private readonly ILibroRepository _libroRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LibroService(ILibroRepository libroRepository, IUnitOfWork unitOfWork)
    {
        _libroRepository = libroRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    ///     Obtiene todos los libros.
    /// </summary>
    /// <returns>Lista de libros.</returns>
    public async Task<IQueryable<Libro>> GetAllLibros() => await _libroRepository.GetAll();

    /// <summary>
    ///     Obtiene la lista de libros paginados, con su número de página, tamaño de paginación, total elementos,
    ///     total páginas, y los elementos de la lista.
    /// </summary>
    /// <param name="records">La lista de libros.</param>
    /// <param name="pageNumber">Número de página.</param>
    /// <param name="pageSize">Cantidad de elementos mostrados por página.</param>
    /// <returns>Lista de libros en formato paginado.</returns>
    public async Task<PagedResult<Libro>> GetRecordsPagedResult(IQueryable<Libro> records, int pageNumber, int pageSize)
    {
        return await _libroRepository.GetRecordsPagedResult(records, pageNumber, pageSize);
    }

    /// <summary>
    ///     Obtiene 1 libro mediante su ID.
    /// </summary>
    /// <param name="id">ID del libro.</param>
    /// <returns>El libro correspondiente a su ID.</returns>
    public async Task<Libro> GetLibroById(int? id)
    {
        return await _libroRepository.GetById(id);
    }

    /// <summary>
    ///     Inserta un libro.
    /// </summary>
    /// <param name="libro">El libro.</param>
    public async Task AddLibro(Libro libro)
    {
        await _libroRepository.Add(libro);
    }

    /// <summary>
    ///     Actualiza un libro.
    /// </summary>
    /// <param name="libro">El libro.</param>
    public async Task UpdateLibro(Libro libro)
    {
        await _libroRepository.Update(libro);
    }

    /// <summary>
    ///     Elimina un libro.
    /// </summary>
    /// <param name="id">ID del libro.</param>
    /// <returns>'true' si el libro se ha eliminado correctamente, 'false' en caso contrario.</returns>
    public async Task<bool> DeleteLibro(int? id)
    {
        if (id == null)
        {
            throw new CRUDException("No se pudo borrar el libro");
        }

        return await _libroRepository.Delete(id);
    }

    /// <summary>
    ///     Elimina 1 o más libros.
    /// </summary>
    /// <param name="ids">Lista de IDs de los libros. Los checkboxes seleccionados en la vista almacenan sus IDs.</param>
    /// <returns>'true' si los libros seleccionados se han eliminado correctamente, 'false' en caso contrario.</returns>
    public async Task<bool> DeleteMultipleLibros(int[] ids)
    {
        if (ids == null || ids.Length == 0)
        {
            throw new CRUDException("No se pudo borrar múltiples libros");
        }

        return await _libroRepository.DeleteMultiple(ids);
    }

    /// <summary>
    ///     Realiza un filtro de búsqueda de libros.
    /// </summary>
    /// <param name="predicate">Condiciones de los filtros de búsqueda a aplicar.</param>
    /// <returns>Lista de libros que cumplen con el criterio de búsqueda indicado.</returns>
    public IQueryable<Libro> SearchLibro(Expression<Func<Libro, bool>> predicate)
    {
        try
        {
            return _libroRepository.Search(predicate);
        }
        catch (Exception)
        {
            throw new SearchException("Error en el filtro de búsqueda de libros");
        }
    }

    /// <summary>
    ///     Realiza un filtro de búsqueda de libros.
    /// </summary>
    /// <param name="queryType">El tipo de la consulta a realizar en la búsqueda. Se utiliza para mostrar el 
    ///     listado de libros sin filtrado de búsqueda</param>
    /// <param name="predicate">Condiciones de los filtros de búsqueda a aplicar.</param>
    /// <returns>Lista de libros que cumplen con el criterio de búsqueda indicado.</returns>
    public IQueryable<Libro> SearchLibro(IQueryable<Libro> queryType, Expression<Func<Libro, bool>> predicate)
    {
        try
        {
            return _libroRepository.Search(queryType, predicate);
        }
        catch (Exception)
        {
            throw new SearchException("Error en el filtro de búsqueda de libros");
        }
    }

    /// <summary>
    ///     Carga las relaciones "Autor" y "Categoría" de la entidad "Libro".
    /// </summary>
    /// <returns>Lista de libros con las relaciones "Autor" y "Categorías" cargadas, con esto podremos acceder a 
    /// sus propiedades desde la entidad "Libro".</returns>
    public IQueryable<Libro> GetLibrosConAutoresCategorias() => _libroRepository.GetLibrosConAutoresCategorias();

    /// <summary>
    ///     Asigna a la entidad "Libro" la imagen en binario desde el formulario de "Crear/Modificar" libro.
    /// </summary>
    /// <param name="Imagen">La imagen subida en el formulario.</param>
    /// <param name="libro">La entidad Libro para asignar la imagen cargada del formulario.</param>
    /// <param name="ImagenActual">Si ya existe imagen asignada, la muestro en el formulario. En caso contrario, la sobreescribo 
    /// por la nueva imagen a subir.</param>
    /// <returns>Una tupla con la entidad "Libro" y un valor "string" con el mensaje de la validación en caso de superar 
    /// el tamaño máximo admitido a la hora de cargar la imagen (si se diera el caso). En caso contrario, no mostrará ningún mensaje,
    /// devolviendo un valor 'null'.</returns>
    public async Task<(Libro?, string?)> SetBinaryImage(IFormFile? Imagen, Libro? libro, string? ImagenActual = null)
    {
        if (Imagen?.Length > 0)
        {
            if (Imagen.Length > 2097152) // 2MB en bytes
            {
                return (null, "La imagen debe ser menor a 2 MB.");
            }

            await using var memoryStream = new MemoryStream();
            await Imagen.CopyToAsync(memoryStream);
            libro.Imagen = memoryStream?.ToArray() ?? [];
        }
        else if (!string.IsNullOrEmpty(ImagenActual))
        {
            libro.Imagen = Convert.FromBase64String(ImagenActual);
        }

        return (libro, null);
    }
}
