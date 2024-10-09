using FluentValidation;
using BibliotecaNET8.Application.Resources;
using BibliotecaNET8.Application.DTOs.Cliente;

namespace BibliotecaNET8.Application.Validators
{
    /// <summary>
    ///     Validaciones correspondientes a la entidad "Cliente"
    /// </summary>
    public class ClienteValidator : AbstractValidator<ClienteDTO>
    {
        public ClienteValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(cliente => cliente.Nombre)
                .NotEmpty().WithMessage(ValidationMessages.ClienteNombreRequired)
                .MaximumLength(100).WithMessage(ValidationMessages.ClienteNombreStringLength);

            RuleFor(cliente => cliente.Apellido)
                .MaximumLength(100).WithMessage(ValidationMessages.ClienteApellidosStringLength);

            RuleFor(cliente => cliente.Email)
                .EmailAddress().WithMessage(ValidationMessages.ClienteEmailPattern)
                .When(cliente => !string.IsNullOrEmpty(cliente.Email));

            RuleFor(cliente => cliente.Telefono)
                .MaximumLength(20).WithMessage(ValidationMessages.ClienteTelefonosStringLength)
                .When(cliente => !string.IsNullOrEmpty(cliente.Email));
        }
    }
}
