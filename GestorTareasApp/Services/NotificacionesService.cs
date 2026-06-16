
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorTareasApp.Services
{
    public class NotificacionesService
    {
        public async Task MostrarSnackbar(string mensaje)
        {
            var opciones = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#1F2937"),
                TextColor = Colors.White,
                CornerRadius = 12,
                ActionButtonTextColor = Color.FromArgb("#93C5FD")
            };

            var snackbar = Snackbar.Make(
                mensaje,
                duration: TimeSpan.FromSeconds(3),
                visualOptions: opciones
            );

            await snackbar.Show();
        }

        public async Task MostrarError(string mensaje)
        {
            var opciones = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#991B1B"),
                TextColor = Colors.White,
                CornerRadius = 12,
                ActionButtonTextColor = Colors.White
            };

            var snackbar = Snackbar.Make(
                mensaje,
                duration: TimeSpan.FromSeconds(3),
                visualOptions: opciones
            );

            await snackbar.Show();
        }
    }
}

