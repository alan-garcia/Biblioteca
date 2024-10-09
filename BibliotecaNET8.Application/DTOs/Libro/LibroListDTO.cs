namespace BibliotecaNET8.Application.DTOs.Libro;

public class LibroListDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string? ISBN { get; set; }
    public DateTime FechaPublicacion { get; set; } = DateTime.Now;
    public byte[]? Imagen { get; set; }
    public int AutorId { get; set; }
    public int CategoriaId { get; set; }

    public virtual Domain.Entities.Autor Autor { get; set; }
    public virtual Domain.Entities.Categoria Categoria { get; set; }
}
