using FluentValidation;
using GestorTareas.DTOs.Tareas;

namespace GestorTareasAPI.Validators
{
    public class EditarTareaValidator : AbstractValidator<EditarTareaDTO>
    {
        public EditarTareaValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

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
        }
    }
}
