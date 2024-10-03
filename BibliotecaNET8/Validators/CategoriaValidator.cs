using FluentValidation;
using BibliotecaNET8.Resources;
using BibliotecaNET8.ViewModels.Categoria;

namespace BibliotecaNET8.Validators
{
    public class CategoriaValidator : AbstractValidator<CategoriaVM>
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
