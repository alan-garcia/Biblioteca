using BibliotecaNET8.Resources;
using FluentValidation;
using BibliotecaNET8.ViewModels.Cliente;

namespace BibliotecaNET8.Validators
{
    public class ClienteValidator : AbstractValidator<ClienteVM>
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
