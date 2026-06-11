using CommunityToolkit.Maui.Alerts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorTareasApp.Services
{
    public class NotificacionesService
    {
        public async Task MostrarToast(string mensaje)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                var toast = Toast.Make(mensaje);
                return toast.Show();
            });
        }
    }
}
