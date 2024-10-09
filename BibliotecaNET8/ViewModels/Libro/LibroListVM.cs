using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.Web.ViewModels.Libro;

public class LibroListVM
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

    public virtual Domain.Entities.Autor Autor { get; set; }
    public virtual Domain.Entities.Categoria Categoria { get; set; }
}
