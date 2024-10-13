namespace BibliotecaNET8.Domain;

/// <summary>
///     Estructura de una entidad de tipo "paginada" a la hora de implementar el 
///     filtro de búsqueda con paginación
/// </summary>
/// <typeparam name="T">La clase de tipo entidad a aplicar.</typeparam>
public class PagedResult<T> where T : class
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int StartPage { get; set; }
    public int EndPage { get; set; }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
