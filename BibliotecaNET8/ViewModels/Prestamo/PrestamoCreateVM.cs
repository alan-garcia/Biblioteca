using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.ViewModels.Prestamo;

public class PrestamoCreateVM
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

    public IEnumerable<SelectListItem> Libros { get; set; }
    public IEnumerable<SelectListItem> Clientes { get; set; }
}
