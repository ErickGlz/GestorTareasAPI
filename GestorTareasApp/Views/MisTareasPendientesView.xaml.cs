using GestorTareasApp.ViewModels;

namespace GestorTareasApp.Views;

public partial class MisTareasPendientesView : ContentPage
{
	public MisTareasPendientesView(TareasViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

    }
}