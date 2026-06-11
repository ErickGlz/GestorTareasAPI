using GestorTareasApp.ViewModels;

namespace GestorTareasApp.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}