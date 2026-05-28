using GestorTareasApp.ViewModels;

namespace GestorTareasApp.Views;

public partial class MisTareasTodasView : ContentPage
{
	public MisTareasTodasView(TareasViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}