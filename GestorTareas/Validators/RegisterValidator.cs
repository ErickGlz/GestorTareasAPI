using FluentValidation;
using GestorTareasAPI.Models.DTOs;

namespace GestorTareasAPI.Validators
{
    public class RegisterValidator: AbstractValidator<RegistroDTO>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .WithMessage("El nombre es obligatorio")
                .MaximumLength(100);

            RuleFor(x => x.Correo)
                .NotEmpty()
                .WithMessage("El correo es obligatorio")
                .EmailAddress()
                .WithMessage("Correo inválido");

            RuleFor(x => x.Contrasena)
                .NotEmpty()
                .WithMessage("La contraseña es obligatoria")
                .MinimumLength(6)
                .WithMessage("La contraseña debe tener al menos 6 caracteres");
        }
    }
}
