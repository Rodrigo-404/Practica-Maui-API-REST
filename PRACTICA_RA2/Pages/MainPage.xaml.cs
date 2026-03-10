using System.Threading;

using PRACTICA_RA2.Models;
using PRACTICA_RA2.ViewModels;

namespace PRACTICA_RA2.Pages;

public partial class MainPage : ContentPage
{

	int count = 0;
	private CancionesViewModel viewModel;
    private int _currentSpan = 0;
    private bool mas730 = false;
    private bool menos729 = false;

    public MainPage(CancionesViewModel vm) 
	{
		
		InitializeComponent();
		viewModel = vm;
		BindingContext = viewModel;

    }

    private async void OnCancionSeleccionada(object sender, SelectionChangedEventArgs e)
    {
        var cancionSeleccionada = (Cancion)e.CurrentSelection.FirstOrDefault();
        if (cancionSeleccionada == null) return;

        ((CollectionView)sender).SelectedItem = null;

        await Shell.Current.GoToAsync(nameof(DetalleCancionPage), new Dictionary<string, object>
        {
            ["Cancion"] = cancionSeleccionada
        });
    }

    private async void OnIrANuevaTareaClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            await btn.ScaleTo(0.95, 80, Easing.CubicOut);
            await btn.ScaleTo(1.0, 80, Easing.CubicIn);
        }
        await Shell.Current.GoToAsync("///nueva");
    }

    /* CALCULA Y CAMBIA LA DISPOSICIÓN DE LOS ELEMENTOS DEL GRID DE LAS CANCIONES SEGÚN EL ANCHO DE LA PANTALLA */
    private double _ultimaEscala = -1;

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        double nuevaEscala = width >= 730 ? 1 : 0.8;

        if (_ultimaEscala != nuevaEscala)
        {
            _ultimaEscala = nuevaEscala;
            viewModel.CambiarEscala(nuevaEscala);
        }
    }

}