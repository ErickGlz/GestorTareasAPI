using FluentValidation;
using GestorTareasAPI.Models.DTOs;

namespace GestorTareasAPI.Validators
{
    public class CrearTareaValidator : AbstractValidator<CrearTareaDTO>
    {
        public CrearTareaValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("El título es obligatorio")
                .MaximumLength(100);

            RuleFor(x => x.Descripcion)
                .NotEmpty()
                .WithMessage("La descripción es obligatoria");

            RuleFor(x => x.Prioridad)
                .NotEmpty()
                .WithMessage("La prioridad es obligatoria");

            RuleFor(x => x.FechaLimite)
                .GreaterThan(DateTime.Now)
                .WithMessage("La fecha debe ser futura");
        }
    }
}
