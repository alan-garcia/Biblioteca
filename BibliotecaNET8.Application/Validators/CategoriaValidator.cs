using FluentValidation;
using BibliotecaNET8.Application.Resources;
using BibliotecaNET8.Application.DTOs.Categoria;

namespace BibliotecaNET8.Application.Validators
{
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
