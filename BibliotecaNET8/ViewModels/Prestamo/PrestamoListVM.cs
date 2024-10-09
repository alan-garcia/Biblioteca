using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.Web.ViewModels.Prestamo;

public class PrestamoListVM
{
    public int Id { get; set; }

    [DisplayName("Fecha Préstamo")]
    [DataType(DataType.Date)]
    public DateTime FechaPrestamo { get; set; } = DateTime.Now;

    [DisplayName("Fecha Devolución")]
    [DataType(DataType.Date)]
    public DateTime? FechaDevolucion { get; set; } = DateTime.Now;

    [DisplayName("Libro")]
    public int LibroId { get; set; }

    [DisplayName("Cliente")]
    public int ClienteId { get; set; }

    public virtual Domain.Entities.Libro Libro { get; set; }
    public virtual Domain.Entities.Cliente Cliente { get; set; }
}
