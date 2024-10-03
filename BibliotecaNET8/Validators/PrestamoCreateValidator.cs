using FluentValidation;
using BibliotecaNET8.Resources;
using BibliotecaNET8.ViewModels.Prestamo;

namespace BibliotecaNET8.Validators
{
    public class PrestamoCreateValidator : AbstractValidator<PrestamoCreateVM>
    {
        public PrestamoCreateValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(prestamo => prestamo.FechaPrestamo)
                .NotEmpty().WithMessage(ValidationMessages.PrestamoFechaPrestamoRequired);

            RuleFor(prestamo => prestamo.FechaDevolucion)
                .GreaterThanOrEqualTo(prestamo => prestamo.FechaPrestamo)
                .WithMessage(ValidationMessages.PrestamoFechaDevolucionAnterior);
        }
    }
}
