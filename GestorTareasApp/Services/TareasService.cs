using GestorTareasApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace GestorTareasApp.Services
{
    public class TareasService
    {
        private readonly HttpClient httpClient;

        public TareasService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        private async Task AgregarToken()
        {
            var token = await SecureStorage.Default.GetAsync("token");

            httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrWhiteSpace(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<TareaModel>> GetTareas()
        {
            try
            {
                await AgregarToken();

                var response = await httpClient.GetAsync("api/tareas");

                if (!response.IsSuccessStatusCode)
                    return new List<TareaModel>();

                return await response.Content.ReadFromJsonAsync<List<TareaModel>>()
                       ?? new List<TareaModel>();
            }
            catch
            {
                return new List<TareaModel>();
            }
        }

        public async Task<bool> CrearTarea(TareaModel tarea)
        {
            try
            {
                await AgregarToken();

                var dto = new
                {
                    tarea.Titulo,
                    tarea.Descripcion,
                    tarea.FechaLimite,
                    tarea.Prioridad,
                    tarea.ImagenUrl
                };

                var response = await httpClient.PostAsJsonAsync("api/tareas", dto);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditarTarea(TareaModel tarea)
        {
            try
            {
                await AgregarToken();

                var dto = new
                {
                    tarea.Id,
                    tarea.Titulo,
                    tarea.Descripcion,
                    tarea.FechaLimite,
                    tarea.Prioridad,
                    tarea.Completada,
                    tarea.ImagenUrl
                };

                var response = await httpClient.PutAsJsonAsync("api/tareas", dto);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarTarea(int id)
        {
            try
            {
                await AgregarToken();

                var response = await httpClient.DeleteAsync($"api/tareas/{id}");

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
