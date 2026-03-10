using PRACTICA_RA2.ViewModels;

namespace PRACTICA_RA2.Pages;

public partial class InfoPage : ContentPage
{
	private CancionesViewModel viewModel;
	public InfoPage(CancionesViewModel vm)
	{
		InitializeComponent();
        viewModel = vm;
        BindingContext = this;

    }

    private async void OnRegresarClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///main");
	}

	public int CancionesTotales
	{
		get => viewModel.NumeroCanciones;
	}

	public int ArtistasTotales
	{
		get => viewModel.NumeroArtistas;
	}

	public int CancionesFavoritas
	{
		get => viewModel.NumeroCancionesFavoritas;
	}
}