using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.ViewModels.Libro;

public class LibroListVM
{
    public int Id { get; set; }

    [DisplayName("Título")]
    //[Required(ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "LibroTituloRequired")]
    //[StringLength(150, ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "LibroTituloStringLength")]
    public string Titulo { get; set; }

    //[StringLength(13, ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "LibroISBNStringLength")]
    public string? ISBN { get; set; }

    [DisplayName("Fecha de Publicación")]
    //[DataType(DataType.Date)]
    public DateTime FechaPublicacion { get; set; } = DateTime.Now;

    public byte[]? Imagen { get; set; }

    [DisplayName("Autor")]
    public int AutorId { get; set; }

    [DisplayName("Categoría")]
    public int CategoriaId { get; set; }

    public virtual Models.Autor Autor { get; set; }
    public virtual Models.Categoria Categoria { get; set; }
}
