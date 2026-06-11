using GestorTareasAPI.DTOs.Tareas;
using GestorTareasAPI.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace GestorTareasAPI.Services
{
    public class AuthService
    {
        private readonly RegistrotareasContext context;
        private readonly IConfiguration configuration;

        public AuthService(
            RegistrotareasContext context,
            IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<bool> Registrar(RegistroDTO dto)
        {
            var existe = await context.Usuarios
                .AnyAsync(x => x.Correo == dto.Correo);

            if (existe)
                return false;

            var usuario = new Usuarios
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Contrasena = dto.Contrasena,
                FechaRegistro = DateTime.Now
            };

            context.Usuarios.Add(usuario);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<string?> Login(LoginDTO dto)
        {
            var usuario = await context.Usuarios
                .FirstOrDefaultAsync(x =>
                    x.Correo == dto.Correo &&
                    x.Contrasena == dto.Contrasena);

            if (usuario == null)
                return null;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Correo)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var credenciales = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credenciales);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

