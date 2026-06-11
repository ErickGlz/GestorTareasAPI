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

        public TareasService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<TareaModel>> GetTareas()
        {
            var response =
                await httpClient.GetFromJsonAsync<List<TareaModel>>("api/tareas");

            return response ?? new List<TareaModel>();
        }

        public async Task<bool> CrearTarea(TareaModel tarea)
        {
            var response =
                await httpClient.PostAsJsonAsync("api/tareas", tarea);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditarTarea(TareaModel tarea)
        {
            var response =
                await httpClient.PutAsJsonAsync("api/tareas", tarea);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarTarea(int id)
        {
            var response =
                await httpClient.DeleteAsync($"api/tareas/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
