using Microsoft.AspNetCore.Mvc.Rendering;

namespace BibliotecaNET8.Application.DTOs.Prestamo;

public class PrestamoCreateDTO
{
    public int Id { get; set; }
    public DateTime FechaPrestamo { get; set; } = DateTime.Now;
    public DateTime? FechaDevolucion { get; set; } = DateTime.Now;
    public int LibroId { get; set; }
    public int ClienteId { get; set; }
    public IEnumerable<SelectListItem> Libros { get; set; }
    public IEnumerable<SelectListItem> Clientes { get; set; }
}
