using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.ViewModels.Autor;

public class AutorVM
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string? Apellido { get; set; }

    [DisplayName("Fecha de Nacimiento")]
    [DataType(DataType.Date)]
    public DateTime FechaNacimiento { get; set; }
}
