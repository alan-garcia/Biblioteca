using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.ViewModels.Libro;

public class LibroCreateVM
{
    public int Id { get; set; }

    [DisplayName("Título")]
    public string Titulo { get; set; }

    public string? ISBN { get; set; }

    [DisplayName("Fecha de Publicación")]
    [DataType(DataType.Date)]
    public DateTime FechaPublicacion { get; set; } = DateTime.Now;

    public byte[]? Imagen { get; set; }

    [DisplayName("Autor")]
    public int AutorId { get; set; }

    [DisplayName("Categoría")]
    public int CategoriaId { get; set; }

    public IEnumerable<SelectListItem> Autores { get; set; }
    public IEnumerable<SelectListItem> Categorias { get; set; }
}
