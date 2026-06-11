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
        private readonly NotificacionesService notificacionesService;

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
        private bool notificacionesActivadas = true;
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
        public TareasViewModel(
        TareasService service,
        NotificacionesService notificacionesService)
        {
            this.service = service;
            this.notificacionesService = notificacionesService;
            this.notificacionesService = notificacionesService;

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
        public async Task IrEditarTarea(TareaModel tarea)
        {
            if (tarea == null)
                return;

            NuevaTarea = tarea;

            ImagenSeleccionada = tarea.ImagenUrl;

            await Shell.Current.GoToAsync("//NuevaTareaView");
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
        public async Task IrAjustes()
        {
            await Shell.Current.GoToAsync("//AjustesView");
        }
        [RelayCommand]
        public async Task Regresar()
        {
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
        public async Task EliminarTareaSeleccionada()
        {
            if (TareaSeleccionada == null)
                return;

            bool confirmar = await App.Current.MainPage.DisplayAlert(
                "Confirmar",
                $"¿Eliminar la tarea {TareaSeleccionada.Titulo}?",
                "Sí",
                "No");

            if (!confirmar)
                return;

            var resultado = await service.EliminarTarea(TareaSeleccionada.Id);

            if (!resultado)
            {
                Mensaje = "Error al eliminar tarea";
                return;
            }

            await CargarTareas();

            await Shell.Current.GoToAsync("//MisTareasTodasView");
        }
        [RelayCommand]
        public async Task AbrirEditarTarea()
        {
            if (TareaSeleccionada == null)
                return;

            NuevaTarea = new TareaModel
            {
                Id = TareaSeleccionada.Id,
                Titulo = TareaSeleccionada.Titulo,
                Descripcion = TareaSeleccionada.Descripcion,
                FechaLimite = TareaSeleccionada.FechaLimite,
                Prioridad = TareaSeleccionada.Prioridad,
                Completada = TareaSeleccionada.Completada,
                ImagenUrl = TareaSeleccionada.ImagenUrl,
                FechaCreacion = TareaSeleccionada.FechaCreacion
            };

            ImagenSeleccionada = NuevaTarea.ImagenUrl;

            await Shell.Current.GoToAsync("//EditarTareaView");
        }

        [RelayCommand]
        public async Task GuardarCambios()
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

            if (NuevaTarea.FechaLimite < DateTime.Today)
            {
                Mensaje = "La fecha límite no puede ser anterior a hoy";
                return;
            }

            var resultado = await service.EditarTarea(NuevaTarea);

            if (!resultado)
            {
                Mensaje = "Error al actualizar tarea";
                await notificacionesService.MostrarToast(Mensaje);
                return;
            }

            await CargarTareas();

            TareaSeleccionada = NuevaTarea;

            Mensaje = "Tarea actualizada correctamente";
            await notificacionesService.MostrarToast(Mensaje);
            await Shell.Current.GoToAsync("//VerTareaView");
        }
        [RelayCommand]
        public async Task CompletarTarea()
        {
            if (TareaSeleccionada == null)
            return;

            TareaSeleccionada.Completada = true;

            var resultado = await service.EditarTarea(TareaSeleccionada);

            if (!resultado)
            {
                Mensaje = "Error al completar la tarea";
                await notificacionesService.MostrarToast(Mensaje);
                return;
            }

            await CargarTareas();

            Mensaje = "Tarea completada";
            await notificacionesService.MostrarToast(Mensaje);
        }
        [RelayCommand]
        public async Task CrearTarea()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                await notificacionesService.MostrarToast("Sin conexión a Internet");
                return;
            }
            if (string.IsNullOrWhiteSpace(NuevaTarea.Titulo))
            {
                Mensaje = "Ingrese un título";
                await notificacionesService.MostrarToast(Mensaje);
                return;
            }

            if (string.IsNullOrWhiteSpace(NuevaTarea.Descripcion))
            {
                Mensaje = "Ingrese una descripción";

                return;
            }

            if (NuevaTarea.FechaLimite < DateTime.Today)
            {
                Mensaje = "La fecha límite no puede ser anterior a hoy";
                return;
            }

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
                await notificacionesService.MostrarToast(Mensaje);
            }
            else
            {
                var resultado = await service.EditarTarea(NuevaTarea);

                if (!resultado)
                {
                    Mensaje = "Error al actualizar tarea";
                    await notificacionesService.MostrarToast(Mensaje);
                    return;
                }

                Mensaje = "Tarea actualizada correctamente";
                await notificacionesService.MostrarToast(Mensaje);
            }

            await CargarTareas();

            NuevaTarea = new TareaModel();

            ImagenSeleccionada = "";

            await Shell.Current.GoToAsync("//MisTareasTodasView");
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
        [RelayCommand]
        public async Task CambiarNotificaciones()
        {
            if (NotificacionesActivadas)
            {
                await notificacionesService.MostrarToast("Notificaciones activadas");
            }
            else
            {
                await notificacionesService.MostrarToast("Notificaciones desactivadas");
            }
        }
    }
}