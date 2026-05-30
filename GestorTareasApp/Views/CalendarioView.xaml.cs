using GestorTareasApp.ViewModels;

namespace GestorTareasApp.Views;

public partial class CalendarioView : ContentPage
{
	public CalendarioView(TareasViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

    }
}