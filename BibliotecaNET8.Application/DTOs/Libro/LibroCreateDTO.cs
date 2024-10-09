using Microsoft.AspNetCore.Mvc.Rendering;

namespace BibliotecaNET8.Application.DTOs.Libro;

public class LibroCreateDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string? ISBN { get; set; }
    public DateTime FechaPublicacion { get; set; } = DateTime.Now;
    public byte[]? Imagen { get; set; }
    public int AutorId { get; set; }
    public int CategoriaId { get; set; }

    public IEnumerable<SelectListItem> Autores { get; set; }
    public IEnumerable<SelectListItem> Categorias { get; set; }
}
