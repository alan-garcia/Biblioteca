using BibliotecaNET8.Resources;
using FluentValidation;
using BibliotecaNET8.ViewModels.Autor;

namespace BibliotecaNET8.Validators
{
    public class AutorValidator : AbstractValidator<AutorVM>
    {
        public AutorValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(autor => autor.Nombre)
                .NotEmpty().WithMessage(ValidationMessages.AutorNombreRequired)
                .Length(3, 100).WithMessage(ValidationMessages.AutorNombreStringLength);

            RuleFor(autor => autor.Apellido)
                .MaximumLength(100).WithMessage(ValidationMessages.AutorApellidosStringLength);

            RuleFor(autor => autor.FechaNacimiento)
                .LessThan(DateTime.Now).WithMessage(ValidationMessages.AutorFechaNacimientoFuture);
        }
    }
}
