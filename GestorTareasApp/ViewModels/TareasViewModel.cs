using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestorTareasApp.Models;
using GestorTareasApp.Services;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;

namespace GestorTareasApp.ViewModels
{
    public partial class TareasViewModel : ObservableObject
    {
        private readonly TareasService service;

        public ObservableCollection<TareaModel> Tareas { get; set; } = new();

        public IEnumerable<TareaModel> TareasTodas =>
            Tareas;

        public IEnumerable<TareaModel> TareasPendientes =>
            Tareas.Where(x => x.Completada == false);

        public IEnumerable<TareaModel> TareasCompletadas =>
            Tareas.Where(x => x.Completada == true);
        public IEnumerable<TareaModel> RecordatoriosTodos =>
    Tareas.OrderBy(x => x.FechaCreacion);

        public IEnumerable<TareaModel> RecordatoriosProximos =>
            Tareas.Where(x => x.FechaCreacion.Date <= DateTime.Now.Date.AddDays(3));
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
        private string mensaje = "";

        [ObservableProperty]
        private string imagenSeleccionada = "";

        [ObservableProperty]
        private TareaModel nuevaTarea = new();

        public TareasViewModel()
        {
            service = new TareasService();

            _ = CargarTareas();
        }

        [RelayCommand]
        public async Task CargarTareas()
        {
            IsLoading = true;

            var lista = await service.GetTareas();

            Tareas.Clear();

            foreach (var item in lista)
            {
                Tareas.Add(item);
            }

            OnPropertyChanged(nameof(TareasTodas));
            OnPropertyChanged(nameof(TareasPendientes));
            OnPropertyChanged(nameof(TareasCompletadas));

            IsLoading = false;
        }
        [RelayCommand]
        public async Task IrRecordatoriosTodos()
        {
            await Shell.Current.GoToAsync("//RecordatoriosTodosView");
        }

        [RelayCommand]
        public async Task IrRecordatoriosProximos()
        {
            await Shell.Current.GoToAsync("//RecordatoriosProximosView");
        }
        [RelayCommand]
        public async Task IrPendientes()
        {
            await Shell.Current.GoToAsync("//MisTareasPendientesView");
        }

        [RelayCommand]
        public async Task IrCompletadas()
        {
            await Shell.Current.GoToAsync("//MisTareasCompletadasView");
        }

        [RelayCommand]
        public async Task IrTodas()
        {
            await Shell.Current.GoToAsync("//MisTareasTodasView");
        }
        [RelayCommand]
        public async Task IrNuevaTarea()
        {
            NuevaTarea = new TareaModel();

            ImagenSeleccionada = "";

            await Shell.Current.GoToAsync("///NuevaTareaView");
        }

        [RelayCommand]
        public async Task Regresar()
        {
            await Shell.Current.GoToAsync("//MisTareasTodasView");
        }

        [RelayCommand]
        public async Task IrEditarTarea(TareaModel tarea)
        {
            if (tarea == null)
                return;

            NuevaTarea = tarea;

            ImagenSeleccionada = tarea.ImagenUrl;

            await Shell.Current.GoToAsync("///NuevaTareaView");
        }

        [RelayCommand]
        public async Task CrearTarea()
        {
            bool resultado;

            if (NuevaTarea.Id == 0)
            {
                NuevaTarea.Completada = false;

                NuevaTarea.FechaCreacion = DateTime.Now;

                resultado = await service.CrearTarea(NuevaTarea);

                if (!resultado)
                {
                    Mensaje = "Error al crear tarea";
                    return;
                }

                Mensaje = "Tarea creada correctamente";
            }
            else
            {
                resultado = await service.EditarTarea(NuevaTarea);

                if (!resultado)
                {
                    Mensaje = "Error al actualizar tarea";
                    return;
                }

                Mensaje = "Tarea actualizada correctamente";
            }

            await CargarTareas();

            NuevaTarea = new TareaModel();

            ImagenSeleccionada = "";

            await Shell.Current.GoToAsync("//MisTareasTodasView");
        }

        [RelayCommand]
        public async Task EliminarTarea(TareaModel tarea)
        {
            if (tarea == null)
                return;

            bool confirm = await App.Current.MainPage.DisplayAlert(
                "Confirmar",
                $"¿Eliminar la tarea {tarea.Titulo}?",
                "Sí",
                "No");

            if (!confirm)
                return;

            var resultado = await service.EliminarTarea(tarea.Id);

            if (!resultado)
            {
                Mensaje = "Error al eliminar tarea";
                return;
            }

            Tareas.Remove(tarea);

            OnPropertyChanged(nameof(TareasTodas));
            OnPropertyChanged(nameof(TareasPendientes));
            OnPropertyChanged(nameof(TareasCompletadas));

            Mensaje = "Tarea eliminada";
        }

        [RelayCommand]
        public async Task SeleccionarImagen()
        {
            var foto = await MediaPicker.PickPhotoAsync();

            if (foto != null)
            {
                ImagenSeleccionada = foto.FullPath;

                NuevaTarea.ImagenUrl = foto.FullPath;
            }
        }
    }
}