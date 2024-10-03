using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.Models;

public class Categoria : BaseEntity
{
    public int Id { get; set; }

    //[Required]
    //[StringLength(100)]
    public string Nombre { get; set; }

    public virtual ICollection<Libro> Libros { get; set; }
}
