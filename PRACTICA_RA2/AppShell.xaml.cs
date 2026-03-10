using PRACTICA_RA2.Pages;

namespace PRACTICA_RA2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DetalleCancionPage), typeof(DetalleCancionPage));
        }
    }
}
