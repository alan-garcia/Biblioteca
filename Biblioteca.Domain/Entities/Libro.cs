using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.Domain.Entities;

public class Libro : BaseEntity
{
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Titulo { get; set; }

    [StringLength(13)]
    public string? ISBN { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaPublicacion { get; set; }

    public byte[]? Imagen { get; set; }

    public int AutorId { get; set; }
    public int CategoriaId { get; set; }
    public virtual Autor Autor { get; set; }
    public virtual Categoria Categoria { get; set; }
}
