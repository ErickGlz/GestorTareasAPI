
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
#if ANDROID
            var context = Android.App.Application.Context;
            Android.Widget.Toast.MakeText(context, mensaje, Android.Widget.ToastLength.Short)?.Show();
#endif

        }
    }
}
