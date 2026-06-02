using GestorTareasApp.ViewModels;

namespace GestorTareasApp.Views;

public partial class VerTareaView : ContentPage
{
	public VerTareaView(TareasViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}