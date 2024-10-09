namespace BibliotecaNET8.Application.DTOs.Autor;

public class AutorDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string? Apellido { get; set; }
    public DateTime FechaNacimiento { get; set; } = DateTime.Now;
}
