namespace BibliotecaNET8.Application.DTOs.Prestamo;

public class PrestamoListDTO
{
    public int Id { get; set; }
    public DateTime FechaPrestamo { get; set; } = DateTime.Now;
    public DateTime? FechaDevolucion { get; set; } = DateTime.Now;
    public int LibroId { get; set; }
    public int ClienteId { get; set; }
    public virtual Domain.Entities.Libro Libro { get; set; }
    public virtual Domain.Entities.Cliente Cliente { get; set; }
}
