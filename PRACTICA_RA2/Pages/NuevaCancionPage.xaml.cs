using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Net.WebSockets;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Storage;
using PRACTICA_RA2.Models;
using PRACTICA_RA2.ViewModels;

namespace PRACTICA_RA2.Pages;

public partial class NuevaCancionPage : ContentPage, INotifyPropertyChanged
{

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string nombre = null)
    { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre)); }

    private CancionesViewModel viewModel;
	private Cancion nuevaCancion;
	private string urlPortada = "dotnet_bot.png";
    public List<string> artistasCancion { get; set; } = new List<string>();

	public NuevaCancionPage(CancionesViewModel vm)
	{
		InitializeComponent();
		viewModel = vm;
		OnPropertyChanged(nameof(urlPortada));

		nuevaCancion = new Cancion
		{
			UrlPortada = urlPortada
		};
		BindingContext = nuevaCancion;
	}

	/* METODO QUE SE ENCARGA DE RECIBIR UNA IMAGEN LOCAL Y MOSTRARLA EN PANTALLA */
    private async void OnPickerClicked(object sender, EventArgs e)
    {
		var res = await FilePicker.PickAsync(new PickOptions
		{
			PickerTitle = "Selecciona una imagen",
			FileTypes = FilePickerFileType.Images
		});

		if (res != null)
		{
			var newFile = Path.Combine(FileSystem.AppDataDirectory, res.FileName);

			using var stream = await res.OpenReadAsync();
			using var newStream = File.OpenWrite(newFile);
			await stream.CopyToAsync(newStream);
			urlPortada = newFile;
			nuevaCancion.UrlPortada = urlPortada;
			OnPropertyChanged(nameof(urlPortada));
			OnPropertyChanged(nameof(nuevaCancion.UrlPortada));
        
		}
	}

	/* JUGAR CON LOS ARTISTAS */
	private void AgregarArtista(object sender, EventArgs e)
	{
		var entry = new Entry { Placeholder = "Nombre del artista"};

		var botonEliminar = new Button
		{
			Text = "—",
			HorizontalOptions = Microsoft.Maui.Controls.LayoutOptions.Start
		};

		botonEliminar.Clicked += EliminarArtista;
		botonEliminar.Style = (Style)Application.Current.Resources["BotonApagadoIcono"];

        var layout = new Grid {};

		layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
		layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

		layout.SetColumn(entry, 0);
		layout.SetColumn(botonEliminar, 1);

		layout.ColumnSpacing = 10;

		layout.Add(entry);
		layout.Add(botonEliminar);
        ContenedorArtistas.Add(layout);
    }

    private void EliminarArtista(object sender, EventArgs e)
	{
		if(sender is Button btn && btn.Parent is Grid layout)
		{
            var entry = layout.Children.OfType<Entry>().FirstOrDefault();
			if(entry != null && !string.IsNullOrWhiteSpace(entry.Text))
			{
				artistasCancion.Remove(entry.Text.Trim());
			}
            ContenedorArtistas.Remove(layout);
		}
	}

	/* GUARDAR/DESCARTAR CAMBIOS */
    private async void OnGuardarClicked(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(nuevaCancion.Titulo))
        {
            await DisplayAlert("Error", "Introduce un titulo para la canción", "Aceptar :/");
            return;
        }

		if(string.IsNullOrEmpty(minutosEntry.Text) || string.IsNullOrEmpty(segundosEntry.Text))
		{
            await DisplayAlert("Error", "Especifica una duración para la canción", "Aceptar");
            return;
        }

		int segundos = 0;
		int minutos = 0;
		if(!int.TryParse(minutosEntry.Text.Trim(), out minutos) || !int.TryParse(segundosEntry.Text.Trim(), out segundos))
		{
            await DisplayAlert("Error", "Introduce una duración en números enteros", "Aceptar");
            return;
        }

		if(int.IsNegative(segundos) || int.IsNegative(minutos) || segundos > 59)
		{
            await DisplayAlert("Error", "Introduce unas medidas de tiempo reales", "Aceptar");
            return;
        }

		/* Recoge los artistas de los elementos hijos del grid */
		artistasCancion = new List<string>();
		foreach(var hijo in ContenedorArtistas.Children)
		{
			if(hijo is Grid grid)
			{
				var entry = grid.Children.OfType<Entry>().FirstOrDefault();
				if(entry != null)
				{
					if(entry.Text == null)
						entry.Text = "";

                    string texto = entry.Text.Trim();

                    TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                    string resultado = ti.ToTitleCase(texto.ToLower());
                    artistasCancion.Add(resultado);
				} 
			}
		}

        bool hayAlgoEnBlanco = false;
        foreach (string art in artistasCancion)
        {
            if (string.IsNullOrEmpty(art))
            {
                hayAlgoEnBlanco = true;
            }
        }

        if (hayAlgoEnBlanco)
        {
            await DisplayAlert("Error", "Introduce un mínimo de un artista y elimine los campos vacíos", "Aceptar");
            return;
        }

        nuevaCancion.Duracion = minutos + ":" + segundos;
		nuevaCancion.EsFavorita = false;
		nuevaCancion.Artistas = artistasCancion;

        await DisplayAlert("Exitoso", "Se ha insertado la cancion con titulo: "+nuevaCancion.Titulo+", \nartistas: "+nuevaCancion.ArtistasString, "Aceptar");
        viewModel.AgregarCancion(nuevaCancion);

        await Shell.Current.GoToAsync("///main");

    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///main");
    }


}