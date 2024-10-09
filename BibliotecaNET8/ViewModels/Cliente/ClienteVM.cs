using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.Web.ViewModels.Cliente;

public class ClienteVM
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string? Apellido { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [DisplayName("Teléfono")]
    public string Telefono { get; set; }
}
