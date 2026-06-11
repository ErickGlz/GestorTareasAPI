using FluentValidation;
using GestorTareasAPI.Models.DTOs;

namespace GestorTareasAPI.Validators
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Correo)
                .NotEmpty()
                .WithMessage("El correo es obligatorio")
                .EmailAddress()
                .WithMessage("Correo inválido");

            RuleFor(x => x.Contrasena)
                .NotEmpty()
                .WithMessage("La contraseña es obligatoria");
        }
    
    }
}
