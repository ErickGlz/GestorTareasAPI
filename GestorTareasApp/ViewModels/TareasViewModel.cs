using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestorTareasApp.Models;
using GestorTareasApp.Services;
using Microsoft.Maui.Storage;
using Plugin.LocalNotification;
using Plugin.LocalNotification.Core.Models;
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
        public ObservableCollection<TareaModel> RecordatoriosHoy { get; set; } = new();
        public ObservableCollection<TareaModel> RecordatoriosSemana { get; set; } = new();
        public ObservableCollection<TareaModel> RecordatoriosFuturos { get; set; } = new();
        public ObservableCollection<TareaModel> TareasCalendario { get; set; } = new();

        public List<string> Prioridades { get; set; } =
        [
            "Alta",
            "Media",
            "Baja"
        ];

        [ObservableProperty] private bool isLoading;
        [ObservableProperty] private bool notificacionesActivadas = true;
        [ObservableProperty] private string mensaje = "";
        [ObservableProperty] private DateTime fechaSeleccionada = DateTime.Today;
        [ObservableProperty] private TareaModel nuevaTarea = new();
        [ObservableProperty] private TareaModel tareaSeleccionada = new();
        [ObservableProperty] private bool sinConexion;
        [ObservableProperty]
        private string imagenSeleccionada = "";
        public TareasViewModel(
            TareasService service,
            NotificacionesService notificacionesService)
        {
            this.service = service;
            this.notificacionesService = notificacionesService;

            Task.Run(async () => await CargarTareas());
        }

        partial void OnFechaSeleccionadaChanged(DateTime value)
        {
            FiltrarCalendario();
        }

        public async Task CargarTareas()
        {
            try
            {
                var profiles = Connectivity.Current.ConnectionProfiles;
                if (!profiles.Contains(ConnectionProfile.WiFi) &&
                    !profiles.Contains(ConnectionProfile.Cellular))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Sin conexión",
                        "No tienes conexión a Internet",
                        "Aceptar");
                    return;
                }
                IsLoading = true;

                var lista = await service.GetTareas();

                if (lista == null)
                    lista = new List<TareaModel>();

                Tareas.Clear();
                TareasPendientes.Clear();
                TareasCompletadas.Clear();
                RecordatoriosTodos.Clear();
                RecordatoriosHoy.Clear();
                RecordatoriosSemana.Clear();
                RecordatoriosFuturos.Clear();

                foreach (var tarea in lista)
                {
                    if (tarea == null) continue;

                    Tareas.Add(tarea);
                    RecordatoriosTodos.Add(tarea);

                    if (!tarea.Completada)
                        TareasPendientes.Add(tarea);

                    if (tarea.Completada)
                        TareasCompletadas.Add(tarea);

                    if (!tarea.Completada)
                    {
                        if (tarea.FechaLimite.Date == DateTime.Today)
                            RecordatoriosHoy.Add(tarea);
                        else if (tarea.FechaLimite.Date <= DateTime.Today.AddDays(7))
                            RecordatoriosSemana.Add(tarea);
                        else
                            RecordatoriosFuturos.Add(tarea);
                    }
                }

                FiltrarCalendario();
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

        private void FiltrarCalendario()
        {
            TareasCalendario.Clear();

            foreach (var tarea in Tareas)
            {
                if (tarea != null &&
                    tarea.FechaLimite.Date == FechaSeleccionada.Date)
                {
                    TareasCalendario.Add(tarea);
                }
            }
        }

        [RelayCommand]
        public async Task IrEditarTarea(TareaModel tarea)
        {
            try
            {
                if (tarea == null) return;


                NuevaTarea = new TareaModel
                {
                    Id = tarea.Id,
                    Titulo = tarea.Titulo,
                    Descripcion = tarea.Descripcion,
                    FechaLimite = tarea.FechaLimite,
                    Prioridad = tarea.Prioridad,
                    Completada = tarea.Completada,
                    FechaCreacion = tarea.FechaCreacion
                };


                await Shell.Current.GoToAsync("//EditarTareaView");
            }
            catch
            {
                Mensaje = "Error al abrir edición";
            }
        }

        [RelayCommand]
        public async Task VerTarea(TareaModel tarea)
        {
            if (tarea == null) return;

            TareaSeleccionada = tarea;

            await Shell.Current.GoToAsync("//VerTareaView");
        }

        [RelayCommand]
        public async Task IrTodas() =>
            await Shell.Current.GoToAsync("//MisTareasTodasView");

        [RelayCommand]
        public async Task IrPendientes() =>
            await Shell.Current.GoToAsync("//MisTareasPendientesView");

        [RelayCommand]
        public async Task IrCompletadas() =>
            await Shell.Current.GoToAsync("//MisTareasCompletadasView");

        [RelayCommand]
        public async Task IrRecordatoriosTodos() =>
            await Shell.Current.GoToAsync("//RecordatoriosTodosView");

        [RelayCommand]
        public async Task IrRecordatoriosProximos() =>
            await Shell.Current.GoToAsync("//RecordatoriosProximosView");

        [RelayCommand]
        public async Task IrCalendario() =>
            await Shell.Current.GoToAsync("//CalendarioView");

        [RelayCommand]
        public async Task IrNuevaTarea()
        {
            Mensaje = "";
            NuevaTarea = new TareaModel();

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.GoToAsync("//NuevaTareaView");
            });
        }

        [RelayCommand]
        public async Task IrAjustes() =>
            await Shell.Current.GoToAsync("//AjustesView");

        [RelayCommand]
        public async Task Regresar() =>
            await Shell.Current.GoToAsync("//MisTareasTodasView");

        [RelayCommand]
        public async Task EliminarTarea(TareaModel tarea)
        {
            try
            {
                var profiles = Connectivity.Current.ConnectionProfiles;
                if (!profiles.Contains(ConnectionProfile.WiFi) &&
                    !profiles.Contains(ConnectionProfile.Cellular))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Sin conexión",
                        "No tienes conexión a Internet",
                        "Aceptar");
                    return;
                }
                if (tarea == null) return;

                bool confirmar = await App.Current.MainPage.DisplayAlert(
                    "Confirmar",
                    $"¿Eliminar la tarea {tarea.Titulo}?",
                    "Sí",
                    "No");

                if (!confirmar) return;

                var resultado = await service.EliminarTarea(tarea.Id);

                if (!resultado)
                {
                    Mensaje = "Error al eliminar tarea";
                    return;
                }

                await CargarTareas();
            }
            catch
            {
                Mensaje = "Error inesperado al eliminar";
            }
        }

        [RelayCommand]
        public async Task EliminarTareaSeleccionada()
        {

            var profiles = Connectivity.Current.ConnectionProfiles;
            if (!profiles.Contains(ConnectionProfile.WiFi) &&
                !profiles.Contains(ConnectionProfile.Cellular))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Sin conexión",
                    "No tienes conexión a Internet",
                    "Aceptar");
                return;
            }
            if (TareaSeleccionada == null) return;

            bool confirmar = await App.Current.MainPage.DisplayAlert(
                "Confirmar",
                $"¿Eliminar la tarea {TareaSeleccionada.Titulo}?",
                "Sí",
                "No");

            if (!confirmar) return;

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
            if (TareaSeleccionada == null) return;

            NuevaTarea = new TareaModel
            {
                Id = TareaSeleccionada.Id,
                Titulo = TareaSeleccionada.Titulo,
                Descripcion = TareaSeleccionada.Descripcion,
                FechaLimite = TareaSeleccionada.FechaLimite,
                Prioridad = TareaSeleccionada.Prioridad,
                Completada = TareaSeleccionada.Completada,
                FechaCreacion = TareaSeleccionada.FechaCreacion
            };


            await Shell.Current.GoToAsync("//EditarTareaView");
        }

        [RelayCommand]
        public async Task GuardarCambios()
        {
            var profiles = Connectivity.Current.ConnectionProfiles;
            if (!profiles.Contains(ConnectionProfile.WiFi) &&
                !profiles.Contains(ConnectionProfile.Cellular))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Sin conexión",
                    "No tienes conexión a Internet",
                    "Aceptar");
                return;
            }
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

            TareaSeleccionada = Tareas.FirstOrDefault(x => x.Id == NuevaTarea.Id);

            Mensaje = "Tarea actualizada correctamente";
            await notificacionesService.MostrarToast(Mensaje);

            await Shell.Current.GoToAsync("//VerTareaView");
        }
        [RelayCommand]
        public async Task CompletarTarea()
        {
            if (TareaSeleccionada == null) return;

            TareaSeleccionada.Completada = true;

            var resultado = await service.EditarTarea(TareaSeleccionada);

            if (!resultado)
            {
                Mensaje = "Error al completar la tarea";
                return;
            }

            await CargarTareas();

            await Shell.Current.GoToAsync("//MisTareasTodasView");
        }

        [RelayCommand]
        public async Task CrearTarea()
        {
            var profiles = Connectivity.Current.ConnectionProfiles;
            if (!profiles.Contains(ConnectionProfile.WiFi) &&
                !profiles.Contains(ConnectionProfile.Cellular))
            {
                Mensaje = "Sin conexión a Internet";
                return;
            }

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

            var resultado = NuevaTarea.Id == 0
                ? await service.CrearTarea(NuevaTarea)
                : await service.EditarTarea(NuevaTarea);

            if (!resultado)
            {
                Mensaje = "Error al guardar tarea";
                return;
            }

            var permitido = await LocalNotificationCenter.Current.RequestNotificationPermission();

            if (permitido)
            {
                var notification = new NotificationRequest
                {
                    NotificationId = new Random().Next(1, 100000),
                    Title = "Recordatorio",
                    Description = $"La tarea '{NuevaTarea.Titulo}' está próxima a vencer.",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddMinutes(1)

                    }
                };

                await LocalNotificationCenter.Current.Show(notification);
            }

            await CargarTareas();

            NuevaTarea = new TareaModel();

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
        public async Task CerrarSesion()
        {
            bool salir = await Application.Current.MainPage.DisplayAlert(
                "Cerrar sesión",
                "¿Deseas cerrar la sesión?",
                "Sí",
                "No");

            if (!salir) return;

            SecureStorage.Default.Remove("token");
            SecureStorage.Default.Remove("refreshToken");

            await Shell.Current.GoToAsync("//LoginView");
        }
    }
}