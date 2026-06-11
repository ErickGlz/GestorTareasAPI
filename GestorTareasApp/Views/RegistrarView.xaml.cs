using GestorTareasApp.ViewModels;

namespace GestorTareasApp.Views;

public partial class RegistrarView : ContentPage
{
	public RegistrarView(LoginViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}