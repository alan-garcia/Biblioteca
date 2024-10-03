using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.ViewModels.Prestamo;

public class PrestamoListVM
{
    public int Id { get; set; }

    [DisplayName("Fecha Préstamo")]
    [DataType(DataType.Date)]
    public DateTime FechaPrestamo { get; set; } = DateTime.Now;

    [DisplayName("Fecha Devolución")]
    [DataType(DataType.Date)]
    public DateTime? FechaDevolucion { get; set; }

    [DisplayName("Libro")]
    public int LibroId { get; set; }

    [DisplayName("Cliente")]
    public int ClienteId { get; set; }

    public virtual Models.Libro Libro { get; set; }
    public virtual Models.Cliente Cliente { get; set; }
}
