using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.Domain.Entities;

public class Cliente : IBaseEntity
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }

    [StringLength(100)]
    public string? Apellido { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [StringLength(20)]
    public string Telefono { get; set; }

    public virtual ICollection<Libro> Libros { get; set; }
}
