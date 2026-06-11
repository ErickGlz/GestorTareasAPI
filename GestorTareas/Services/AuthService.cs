using GestorTareasAPI.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using GestorTareasAPI.Models.DTOs;

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

        public async Task<LoginResponseDTO?> Login(LoginDTO dto)
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

                var jwt = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credenciales);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            var refreshToken = Guid.NewGuid().ToString();

            context.Refreshtokens.Add(new Refreshtokens
            {
                UsuarioId = usuario.Id,
                Token = refreshToken,
                Expiration = DateTime.Now.AddDays(7),
                Revoked = false
            });

            await context.SaveChangesAsync();

            return new LoginResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }
        public async Task<LoginResponseDTO?> Refresh(string refreshToken)
        {
            var tokenGuardado = await context.Refreshtokens
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x =>
                    x.Token == refreshToken &&
                    !x.Revoked &&
                    x.Expiration > DateTime.Now);

            if (tokenGuardado == null)
                return null;

            var usuario = tokenGuardado.Usuario;

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

            var jwt = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credenciales);

            var nuevoToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new LoginResponseDTO
            {
                Token = nuevoToken,
                RefreshToken = refreshToken
            };
        }
    }
}

