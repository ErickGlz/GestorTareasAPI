using CommunityToolkit.Maui;
using GestorTareasApp.Services;
using GestorTareasApp.ViewModels;
using GestorTareasApp.Views;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace GestorTareasApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri("https://thinner-hedge-absently.ngrok-free.dev/")
            });
            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit();

            builder.Services.AddSingleton<NotificacionesService>();

            builder.Services.AddSingleton<TareasService>();
            builder.Services.AddSingleton<AuthService>();

            builder.Services.AddSingleton<TareasViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<MisTareasTodasView>();

            builder.Services.AddTransient<NuevaTareaView>();
            builder.Services.AddTransient<MisTareasPendientesView>();
            builder.Services.AddTransient<MisTareasCompletadasView>();
            builder.Services.AddTransient<RecordatoriosTodosView>();
            builder.Services.AddTransient<RecordatoriosProximosView>();
            builder.Services.AddTransient<CalendarioView>();
            builder.Services.AddTransient<VerTareaView>();
            builder.Services.AddTransient<EditarTareaView>();
            builder.Services.AddTransient<EliminarTareaView>();
            builder.Services.AddTransient<AjustesView>();

            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<RegistrarView>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
