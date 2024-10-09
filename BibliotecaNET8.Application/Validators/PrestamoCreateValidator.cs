using FluentValidation;
using BibliotecaNET8.Application.Resources;
using BibliotecaNET8.Application.DTOs.Prestamo;

namespace BibliotecaNET8.Application.Validators
{
    public class PrestamoCreateValidator : AbstractValidator<PrestamoCreateDTO>
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
