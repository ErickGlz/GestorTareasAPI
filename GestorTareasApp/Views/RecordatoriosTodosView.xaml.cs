using GestorTareasApp.ViewModels;

namespace GestorTareasApp.Views;

public partial class RecordatoriosTodosView : ContentPage
{
	public RecordatoriosTodosView(TareasViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

    }
}