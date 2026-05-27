using GestorTareasApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GestorTareasApp.Services
{
    public class TareasService
    {
        private readonly HttpClient httpClient;

        private string url = "https://localhost:7267/api/tareas";

        public TareasService()
        {
            httpClient = new HttpClient();
        }

        public async Task<List<TareaModel>> GetTareas()
        {
            var response = await httpClient
                .GetFromJsonAsync<List<TareaModel>>(url);

            return response ?? new List<TareaModel>();
        }

        public async Task<bool> CrearTarea(TareaModel tarea)
        {
            var response = await httpClient
                .PostAsJsonAsync(url, tarea);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditarTarea(TareaModel tarea)
        {
            var response = await httpClient
                .PutAsJsonAsync(url, tarea);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarTarea(int id)
        {
            var response = await httpClient
                .DeleteAsync($"{url}/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
