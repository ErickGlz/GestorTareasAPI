using FluentValidation;
using GestorTareasAPI.DTOs.Tareas;
using GestorTareasAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorTareasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;
        private readonly IValidator<LoginDTO> loginValidator;
        private readonly IValidator<RegistroDTO> registroValidator;

        public AuthController(
            AuthService authService,
            IValidator<LoginDTO> loginValidator,
            IValidator<RegistroDTO> registroValidator)
        {
            this.authService = authService;
            this.loginValidator = loginValidator;
            this.registroValidator = registroValidator;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro(RegistroDTO dto)
        {
            var validacion = await registroValidator.ValidateAsync(dto);

            if (!validacion.IsValid)
            {
                return BadRequest(validacion.Errors);
            }

            var resultado = await authService.Registrar(dto);

            if (!resultado)
            {
                return BadRequest("El correo ya está registrado");
            }

            return Ok("Usuario registrado correctamente");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var validacion = await loginValidator.ValidateAsync(dto);

            if (!validacion.IsValid)
            {
                return BadRequest(validacion.Errors);
            }

            var token = await authService.Login(dto);

            if (token == null)
            {
                return Unauthorized("Correo o contraseña incorrectos");
            }

            return Ok(new
            {
                Token = token
            });
        }
    }
}
