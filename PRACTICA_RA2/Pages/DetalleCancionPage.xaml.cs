using System.Globalization;
using System.Threading;
using PRACTICA_RA2.Models;
using PRACTICA_RA2.ViewModels;

namespace PRACTICA_RA2.Pages;

public partial class DetalleCancionPage : ContentPage, IQueryAttributable
{
    private CancionesViewModel viewModel;
	private Cancion cancion;
    private Cancion cancionOriginal;
    private string urlPortada;
    private string [] duracion;
    public List<string> artistasCancion { get; set; } = new List<string>();

    public DetalleCancionPage(CancionesViewModel vm)
	{
		InitializeComponent();
        viewModel = vm;
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Cancion", out var obj) && obj is Cancion c)
        {
            cancionOriginal = c;

            cancion = new Cancion() {
                Titulo = cancionOriginal.Titulo,
                Artistas = cancionOriginal.Artistas,
                Duracion = cancionOriginal.Duracion,
                UrlPortada =cancionOriginal.UrlPortada,
                EsFavorita = cancionOriginal.EsFavorita
            };

            BindingContext = cancion;

            urlPortada = cancion.UrlPortada;
            artistasCancion = cancion.Artistas;
            duracion = cancion.Duracion.Split(":");
            minutosEntry.Text = duracion[0];
            segundosEntry.Text = duracion[1];

            PintarArtistas();
        }

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
            cancion.UrlPortada = urlPortada;
            OnPropertyChanged(nameof(urlPortada));
            OnPropertyChanged(nameof(cancion.UrlPortada));

        }
    }
    /* ALTERNA SI LA CANCION ES FAVORITA O NO */
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        cancion.EsFavorita = !cancion.EsFavorita;
    }

    /* JUGAR CON LOS ARTISTAS */
    public void PintarArtistas()
    {
        int iteracion = 0;
        foreach (var artista in artistasCancion)
        {
            if (iteracion == 0)
            {
                if (ContenedorArtistas.Children[0] is Grid grid)
                {
                    var entry = grid.Children.OfType<Entry>().FirstOrDefault();
                    if (entry != null)
                    {
                        entry.Text = artista;
                        iteracion++;
                    }
                }
                iteracion++;
            }
            else
            {
                AgregarArtista(artista);
            }
        }
    }

    private void AgregarArtista(object sender, EventArgs e)
    {
        var entry = new Entry { Placeholder = "Nombre del artista" };

        var botonEliminar = new Button
        {
            Text = "—",
            HorizontalOptions = Microsoft.Maui.Controls.LayoutOptions.Start
        };

        botonEliminar.Clicked += EliminarArtista;
        botonEliminar.Style = (Style)Application.Current.Resources["BotonApagadoIcono"];

        var layout = new Grid { };

        layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        layout.SetColumn(entry, 0);
        layout.SetColumn(botonEliminar, 1);

        layout.ColumnSpacing = 10;

        layout.Add(entry);
        layout.Add(botonEliminar);
        ContenedorArtistas.Add(layout);
    }

    private void AgregarArtista(string texto)
    {
        var entry = new Entry { Placeholder = "Nombre del artista", Text=texto};

        var botonEliminar = new Button
        {
            Text = "—",
            HorizontalOptions = Microsoft.Maui.Controls.LayoutOptions.Start
        };

        botonEliminar.Clicked += EliminarArtista;
        botonEliminar.Style = (Style)Application.Current.Resources["BotonApagadoIcono"];

        var layout = new Grid { };

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
        if (sender is Button btn && btn.Parent is Grid layout)
        {
            var entry = layout.Children.OfType<Entry>().FirstOrDefault();
            if (entry != null && !string.IsNullOrWhiteSpace(entry.Text))
            {
                artistasCancion.Remove(entry.Text.Trim());
            }
            ContenedorArtistas.Remove(layout);
        }
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(cancion.Titulo))
        {
            await DisplayAlert("Error", "Introduce un titulo para la canción", "Aceptar :/");
            return;
        }

        /* Comprueba los formatos de la duración de la canción */
        if(string.IsNullOrEmpty(minutosEntry.Text) || string.IsNullOrEmpty(segundosEntry.Text))
        {
            await DisplayAlert("Error", "Especifica una duración para la canción", "Aceptar");
            return;
        }

        int segundos = 0;
        int minutos = 0;
        if (!int.TryParse(minutosEntry.Text.Trim(), out minutos) || !int.TryParse(segundosEntry.Text.Trim(), out segundos))
        {
            await DisplayAlert("Error", "Introduce una duración en números enteros", "Aceptar");
            return;
        }

        if (int.IsNegative(segundos) || int.IsNegative(minutos) || segundos > 59)
        {
            await DisplayAlert("Error", "Introduce unas medidas de tiempo reales", "Aceptar");
            return;
        }

        /* Recoge los artistas de los elementos hijos del grid */
        List<string> artistasCancionDetalle = new List<string>();
        foreach (var hijo in ContenedorArtistas.Children)
        {
            if (hijo is Grid grid)
            {
                var entry = grid.Children.OfType<Entry>().FirstOrDefault();
                if (entry != null)
                {
                    if (entry.Text == null)
                        entry.Text = "";

                    string texto = entry.Text.Trim();

                    TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                    string resultado = ti.ToTitleCase(texto.ToLower());
                    artistasCancionDetalle.Add(resultado);
                }
            }
        }

        bool hayAlgoEnBlanco = false;
        foreach (string art in artistasCancionDetalle)
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

        cancion.Duracion = minutos + ":" + segundos;
        cancion.Artistas = artistasCancionDetalle;

        await DisplayAlert("Exitoso", "Se ha insertado la cancion con titulo: " + cancion.Titulo + ", \nartistas: " + cancion.ArtistasString, "Aceptar");
        viewModel.EliminarCancion(cancionOriginal);
        viewModel.AgregarCancion(cancion);

        await Shell.Current.GoToAsync("..");
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert("Confirmación para eliminar", "¿Desea eliminar la canción? Este cambio es irreversible", "Sí", "Cancelar");
        if (!confirmar) return;
        viewModel.EliminarCancion(cancionOriginal);
        await Shell.Current.GoToAsync("///main");

    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert("Confirmación para salir", "¿Desea descartar los cambios? Los cambios se perderán", "Sí", "Cancelar");
        if(confirmar)
        {
            await Shell.Current.GoToAsync("///main");
        }
    }



}