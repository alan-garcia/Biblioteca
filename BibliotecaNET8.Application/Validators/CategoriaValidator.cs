using FluentValidation;
using BibliotecaNET8.Application.Resources;
using BibliotecaNET8.Application.DTOs.Categoria;

namespace BibliotecaNET8.Application.Validators
{
    /// <summary>
    ///     Validaciones correspondientes a la entidad "Categoría"
    /// </summary>
    public class CategoriaValidator : AbstractValidator<CategoriaDTO>
    {
        public CategoriaValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(categoria => categoria.Nombre)
                .NotEmpty().WithMessage(ValidationMessages.CategoriaNombreRequired)
                .MaximumLength(100).WithMessage(ValidationMessages.CategoriaNombreStringLength);
        }
    }
}
