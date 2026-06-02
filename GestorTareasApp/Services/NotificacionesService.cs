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
            var toast = Toast.Make(mensaje);
            await toast.Show();
        }
    }
}
