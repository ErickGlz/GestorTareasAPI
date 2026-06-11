using GestorTareasApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GestorTareasApp.Services
{
    public class AuthService
    {
        private readonly HttpClient httpClient;

        public AuthService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<LoginResponseDTO?> Login(LoginDTO login)
        {
            var response =
                await httpClient.PostAsJsonAsync("api/auth/login", login);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content
                .ReadFromJsonAsync<LoginResponseDTO>();
        }

        
        public async Task<bool> Registro(RegistroDTO registro)
        {
            var response =
                await httpClient.PostAsJsonAsync("api/auth/registro", registro);

            return response.IsSuccessStatusCode;
        }

        public async Task<LoginResponseDTO?> RefreshToken(string refreshToken)
        {
            var dto = new RefreshTokenDTO
            {
                RefreshToken = refreshToken
            };

            var response =
                await httpClient.PostAsJsonAsync("api/auth/refresh", dto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content
                .ReadFromJsonAsync<LoginResponseDTO>();
        }
    }
}
