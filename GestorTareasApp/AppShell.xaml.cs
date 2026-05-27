using GestorTareasApp.Views;

namespace GestorTareasApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("NuevaTareaView", typeof(NuevaTareaView));
        }
    }
}
