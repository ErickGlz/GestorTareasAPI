namespace GestorTareasAPI.Models.DTOs
{
    public class LoginDTO
    {
        public string Correo { get; set; } = "";

        public string Contrasena { get; set; } = "";
    }
    public class RegistroDTO
    {
        public string Nombre { get; set; } = "";

        public string Correo { get; set; } = "";

        public string Contrasena { get; set; } = "";
    }
    public class LoginResponseDTO
    {
        public string Token { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
    }
    public class RefreshTokenDTO
    {
        public string RefreshToken { get; set; } = "";
    }
}
