using GestorTareasApp.ViewModels;

namespace GestorTareasApp.Views;

public partial class RecordatoriosProximosView : ContentPage
{
	public RecordatoriosProximosView(TareasViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

    }
}