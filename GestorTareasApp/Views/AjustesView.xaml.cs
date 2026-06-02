using GestorTareasApp.ViewModels;

namespace GestorTareasApp.Views;

public partial class AjustesView : ContentPage
{
	public AjustesView(TareasViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}