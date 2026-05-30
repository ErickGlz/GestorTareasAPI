using GestorTareasApp.Services;
using GestorTareasApp.ViewModels;
using GestorTareasApp.Views;
using Microsoft.Extensions.Logging;

namespace GestorTareasApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<TareasService>();

            builder.Services.AddSingleton<TareasViewModel>();

            builder.Services.AddSingleton<MisTareasTodasView>();

            builder.Services.AddTransient<NuevaTareaView>();
            builder.Services.AddSingleton<MisTareasPendientesView>();
            builder.Services.AddSingleton<MisTareasCompletadasView>();
            builder.Services.AddSingleton<RecordatoriosTodosView>();
            builder.Services.AddSingleton<RecordatoriosProximosView>();
            builder.Services.AddSingleton<CalendarioView>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
