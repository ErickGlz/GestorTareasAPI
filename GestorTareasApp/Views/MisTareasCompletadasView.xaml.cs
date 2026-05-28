using GestorTareasApp.ViewModels;

namespace GestorTareasApp.Views;

public partial class MisTareasCompletadasView : ContentPage
{
	public MisTareasCompletadasView(TareasViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

    }
}