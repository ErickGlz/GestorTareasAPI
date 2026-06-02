using GestorTareasApp.ViewModels;

namespace GestorTareasApp.Views;

public partial class EliminarTareaView : ContentPage
{
	public EliminarTareaView(TareasViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}