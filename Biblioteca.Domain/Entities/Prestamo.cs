using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.Domain.Entities;

public class Prestamo : BaseEntity
{
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime FechaPrestamo { get; set; }

    [DataType(DataType.Date)]
    public DateTime? FechaDevolucion { get; set; }

    public int LibroId { get; set; }
    public int ClienteId { get; set; }
    public virtual Libro Libro { get; set; }
    public virtual Cliente Cliente { get; set; }
}
