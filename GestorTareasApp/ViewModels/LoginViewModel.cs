using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestorTareasApp.Models.DTOs;
using GestorTareasApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorTareasApp.ViewModels
{
    public partial class LoginViewModel:ObservableObject
    {
        private readonly AuthService authService;
        private readonly NotificacionesService notificacionesService;
        public LoginViewModel(AuthService authService, NotificacionesService notificacionesService)
        {
            this.authService = authService;
            this.notificacionesService = notificacionesService;
        }
        [ObservableProperty]
        private string? nombre;

        [ObservableProperty]
        private string? correo;

        [ObservableProperty]
        private string? contrasena;

        [RelayCommand]
        public async Task IniciarSesion()
        {
            var profiles = Connectivity.Current.ConnectionProfiles;
            if (!profiles.Contains(ConnectionProfile.WiFi) &&
                !profiles.Contains(ConnectionProfile.Cellular))
            {
                await notificacionesService.MostrarError("No tienes conexión a Internet");

                return;
            }
            var login = new LoginDTO
            {
                Correo = Correo,
                Contrasena = Contrasena
            };

            var resultado = await authService.Login(login);

            if (resultado == null)
            {
                await notificacionesService.MostrarSnackbar("Error al iniciar sesión");

                return;
            }

            await SecureStorage.Default.SetAsync("token", resultado.Token);
            await SecureStorage.Default.SetAsync("refreshToken", resultado.RefreshToken);

            await notificacionesService.MostrarSnackbar("inicio de sesion exitoso");

            var vm = IPlatformApplication.Current.Services.GetService<TareasViewModel>();
            if (vm != null)
                await vm.CargarTareas();

            await Shell.Current.GoToAsync("//MisTareasTodasView");
        }
        [RelayCommand]
        public async Task IrRegistrar()
        {
            await Shell.Current.GoToAsync("//IrRegistrarView");
        }
        [RelayCommand]
        public async Task Registrarse()
        {
            var profiles = Connectivity.Current.ConnectionProfiles;
            if (!profiles.Contains(ConnectionProfile.WiFi) &&
                !profiles.Contains(ConnectionProfile.Cellular))
            {
                await notificacionesService.MostrarError("No tienes conexión a Internet");

                return;
            }
            var registro = new RegistroDTO
            {
                Nombre = Nombre,
                Correo = Correo,
                Contrasena = Contrasena
            };

            var resultado = await authService.Registro(registro);

            if (!resultado)
            {
                await notificacionesService.MostrarSnackbar("Usuario Registrado correctamente");

                return;
            }

            await notificacionesService.MostrarSnackbar("Éxito, Usuario registrado correctamente");

            await Shell.Current.GoToAsync("//LoginView");
        }
        [RelayCommand]
        public async Task VolverLogin()
        {
            await Shell.Current.GoToAsync("//LoginView");
        }
       
    }
}
