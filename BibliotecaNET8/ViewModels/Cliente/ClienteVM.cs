using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.ViewModels.Cliente;

public class ClienteVM
{
    public int Id { get; set; }

    //[Required(ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "ClienteNombreRequired")]
    //[StringLength(100, ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "ClienteNombreStringLength")]
    public string Nombre { get; set; }

    //[StringLength(100, ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "ClienteApellidosStringLength")]
    public string? Apellido { get; set; }

    //[EmailAddress(ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "ClienteEmailPattern")]
    //[DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [DisplayName("Teléfono")]
    //[StringLength(20, ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "ClienteTelefonosStringLength")]
    public string Telefono { get; set; }
}
