using FluentValidation;
using BibliotecaNET8.Application.Resources;
using BibliotecaNET8.Application.DTOs.Libro;

namespace BibliotecaNET8.Application.Validators
{
    /// <summary>
    ///     Validaciones correspondientes a la entidad "LibroList"
    /// </summary>
    public class LibroListValidator : AbstractValidator<LibroListDTO>
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
