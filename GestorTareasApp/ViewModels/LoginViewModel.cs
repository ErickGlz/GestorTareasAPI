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

        public LoginViewModel(AuthService authService)
        {
            this.authService = authService;
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
                await Application.Current.MainPage.DisplayAlert(
                    "Sin conexión",
                    "No tienes conexión a Internet",
                    "Aceptar");
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
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Correo o contraseña incorrectos",
                    "Aceptar");

                return;
            }

            await SecureStorage.Default.SetAsync("token", resultado.Token);
            await SecureStorage.Default.SetAsync("refreshToken", resultado.RefreshToken);

            await Application.Current.MainPage.DisplayAlert(
                "Éxito",
                "Inicio de sesión correcto",
                "Aceptar");

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
                await Application.Current.MainPage.DisplayAlert(
                    "Sin conexión",
                    "No tienes conexión a Internet",
                    "Aceptar");
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
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "No fue posible registrar el usuario",
                    "Aceptar");

                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                "Éxito",
                "Usuario registrado correctamente",
                "Aceptar");

            await Shell.Current.GoToAsync("//LoginView");
        }
        [RelayCommand]
        public async Task VolverLogin()
        {
            await Shell.Current.GoToAsync("//LoginView");
        }
       
    }
}
