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

        public ObservableCollection<TareaModel> TareasPendientes { get; set; } = new();

        public ObservableCollection<TareaModel> TareasCompletadas { get; set; } = new();

        public ObservableCollection<TareaModel> RecordatoriosTodos { get; set; } = new();

        public ObservableCollection<TareaModel> RecordatoriosProximos { get; set; } = new();

        public ObservableCollection<TareaModel> TareasCalendario { get; set; } = new();

        public List<string> Prioridades { get; set; } =
        [
            "Alta",
            "Media",
            "Baja"
        ];

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string mensaje = "";

        [ObservableProperty]
        private string imagenSeleccionada = "";

        [ObservableProperty]
        private DateTime fechaSeleccionada = DateTime.Today;

        [ObservableProperty]
        private TareaModel nuevaTarea = new();

        [ObservableProperty]
        private TareaModel tareaSeleccionada = new();
        public TareasViewModel()
        {
            service = new TareasService();

            _ = CargarTareas();
        }

        partial void OnFechaSeleccionadaChanged(DateTime value)
        {
            FiltrarCalendario();
        }

        [RelayCommand]
        public async Task CargarTareas()
        {
            IsLoading = true;

            var lista = await service.GetTareas();

            Tareas.Clear();
            TareasPendientes.Clear();
            TareasCompletadas.Clear();
            RecordatoriosTodos.Clear();
            RecordatoriosProximos.Clear();

            foreach (var tarea in lista)
            {
                Tareas.Add(tarea);

                RecordatoriosTodos.Add(tarea);

                if (!tarea.Completada)
                {
                    TareasPendientes.Add(tarea);
                }

                if (tarea.Completada)
                {
                    TareasCompletadas.Add(tarea);
                }

                if (tarea.FechaCreacion.Date <= DateTime.Today.AddDays(3))
                {
                    RecordatoriosProximos.Add(tarea);
                }
            }

            FiltrarCalendario();

            IsLoading = false;
        }

        private void FiltrarCalendario()
        {
            TareasCalendario.Clear();

            foreach (var tarea in Tareas)
            {
                if (tarea.FechaCreacion.Date == FechaSeleccionada.Date)
                {
                    TareasCalendario.Add(tarea);
                }
            }
        }
        [RelayCommand]
        public async Task VerTarea(TareaModel tarea)
        {
            if (tarea == null)
                return;

            TareaSeleccionada = tarea;

            await Shell.Current.GoToAsync("//VerTareaView");
        }
        [RelayCommand]
        public async Task IrTodas()
        {
            await Shell.Current.GoToAsync("//MisTareasTodasView");
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
        public async Task IrCalendario()
        {
            await Shell.Current.GoToAsync("//CalendarioView");
        }

        [RelayCommand]
        public async Task IrNuevaTarea()
        {
            NuevaTarea = new TareaModel();

            ImagenSeleccionada = "";

            await Shell.Current.GoToAsync("//NuevaTareaView");
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

            await Shell.Current.GoToAsync("//NuevaTareaView");
        }

        [RelayCommand]
        public async Task CrearTarea()
        {
            if (NuevaTarea.Id == 0)
            {
                NuevaTarea.Completada = false;
                NuevaTarea.FechaCreacion = DateTime.Now;

                var resultado = await service.CrearTarea(NuevaTarea);

                if (!resultado)
                {
                    Mensaje = "Error al crear tarea";
                    return;
                }

                Mensaje = "Tarea creada correctamente";
            }
            else
            {
                var resultado = await service.EditarTarea(NuevaTarea);

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

            bool confirmar = await App.Current.MainPage.DisplayAlert(
                "Confirmar",
                $"¿Eliminar la tarea {tarea.Titulo}?",
                "Sí",
                "No");

            if (!confirmar)
                return;

            var resultado = await service.EliminarTarea(tarea.Id);

            if (!resultado)
            {
                Mensaje = "Error al eliminar tarea";
                return;
            }

            await CargarTareas();

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