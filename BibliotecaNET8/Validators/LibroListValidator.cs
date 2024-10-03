using FluentValidation;
using BibliotecaNET8.ViewModels.Libro;
using BibliotecaNET8.Resources;

namespace BibliotecaNET8.Validators
{
    public class LibroListValidator : AbstractValidator<LibroListVM>
    {
        public LibroListValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(libro => libro.Titulo)
                .NotEmpty().WithMessage(ValidationMessages.LibroTituloRequired)
                .MaximumLength(150).WithMessage(ValidationMessages.LibroTituloStringLength);

            RuleFor(libro => libro.ISBN)
                .MaximumLength(13).WithMessage(ValidationMessages.LibroISBNStringLength);

            RuleFor(libro => libro.FechaPublicacion)
                .LessThan(DateTime.Now).WithMessage(ValidationMessages.LibroFechaPublicacionFuture);
        }
    }
}
