using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestorTareasApp.Models;
using GestorTareasApp.Services;
using GestorTareasApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace GestorTareasApp.ViewModels
{
    public partial class TareasViewModel : ObservableObject
    {
        private readonly TareasService service;

        public ObservableCollection<TareaModel> Tareas { get; set; } = new();

        public List<string> Prioridades { get; set; } =
            new()
            {
                "Alta",
                "Media",
                "Baja"
            };

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string imagenSeleccionada;

        [ObservableProperty]
        private string mensaje = "";

        [ObservableProperty]
        private TareaModel nuevaTarea = new();

        public TareasViewModel()
        {
            service = new TareasService();

            CargarTareas();
        }

        [RelayCommand]
        public async Task CargarTareas()
        {
            try
            {
                IsLoading = true;

                var lista = await service.GetTareas();

                Tareas.Clear();

                foreach (var item in lista)
                {
                    Tareas.Add(item);
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task IrNuevaTarea()
        {
            NuevaTarea = new TareaModel();

            await Shell.Current.GoToAsync("///NuevaTareaView");
        }

        [RelayCommand]
        public async Task IrEditarTarea(TareaModel tarea)
        {
            if (tarea == null)
                return;

            NuevaTarea = tarea;

            await Shell.Current.GoToAsync("///NuevaTareaView");
        }

        [RelayCommand]
        public async Task CrearTarea()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NuevaTarea.Titulo))
                {
                    Mensaje = "Ingrese un título";
                    return;
                }

                if (string.IsNullOrWhiteSpace(NuevaTarea.Descripcion))
                {
                    Mensaje = "Ingrese una descripción";
                    return;
                }

                if (string.IsNullOrWhiteSpace(NuevaTarea.Prioridad))
                {
                    Mensaje = "Seleccione una prioridad";
                    return;
                }

                if (NuevaTarea.Id == 0)
                {
                    NuevaTarea.Completada = false;

                    NuevaTarea.FechaCreacion = DateTime.Now;

                    NuevaTarea.ImagenUrl = "";

                    await service.CrearTarea(NuevaTarea);

                    Mensaje = "Tarea creada correctamente";
                }
                else
                {
                    await service.EditarTarea(NuevaTarea);

                    Mensaje = "Tarea actualizada";
                }

                await CargarTareas();

                NuevaTarea = new TareaModel();

                await Shell.Current.GoToAsync("//MisTareasTodasView");
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        [RelayCommand]
        public async Task EliminarTarea(TareaModel tarea)
        {
            try
            {
                bool confirm = await App.Current.MainPage.DisplayAlert(
                    "Confirmar",
                    $"¿Eliminar la tarea {tarea.Titulo}?",
                    "Sí",
                    "No");

                if (!confirm)
                    return;

                await service.EliminarTarea(tarea.Id);

                Tareas.Remove(tarea);

                Mensaje = "Tarea eliminada";
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
        [RelayCommand]
        public async Task SeleccionarImagen()
        {
            try
            {
                var foto = await MediaPicker.PickPhotoAsync();

                if (foto != null)
                {
                    ImagenSeleccionada = foto.FullPath;

                    NuevaTarea.ImagenUrl = foto.FileName;
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}
