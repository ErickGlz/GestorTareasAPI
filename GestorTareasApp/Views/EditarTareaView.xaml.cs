using GestorTareasApp.ViewModels;

namespace GestorTareasApp.Views;

public partial class EditarTareaView : ContentPage
{
	public EditarTareaView(TareasViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

    }
}