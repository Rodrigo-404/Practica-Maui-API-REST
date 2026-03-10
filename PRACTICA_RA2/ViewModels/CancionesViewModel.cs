using PRACTICA_RA2.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PRACTICA_RA2.Services;
using System.Reflection.PortableExecutable;
using System.Diagnostics;

namespace PRACTICA_RA2.ViewModels;

public class CancionesViewModel : INotifyPropertyChanged
{
	public CancionesViewModel(CancionesService service)
	{
		AplicarFiltro();
        _service = service;
        CargarCommand = new Command(async () => await SimularCargaAsync(), () => !EstaOcupado);

    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string nombre = null)
    { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre)); }

    private double _scaleFactor = 1.0;
    private double magnitudImagen = 100;
    private double magnitudCorazon = 30;
    private double tamanoTexto = 16;
    private double paddingElementos = 8;
    private double paddingTarjeta = 15;

    //Gestion tareas carga canciones
    private readonly CancionesService _service;
    private string _mensajeEstado = "Listo";
    private bool _estaOcupado;


    /* Código que se asignara automaticamente a las canciones */
    private int id = 0;

    public ObservableCollection<Cancion> Canciones { get; } = new()
	{
		//Carga canciones
    };

	public int NumeroCanciones => Canciones.Count;
	public int NumeroCancionesFavoritas => Canciones.Count(c => c.EsFavorita);
    public int NumeroArtistas => Canciones
									.SelectMany(c => c.Artistas)
									.Distinct()
									.Count();

    private void NotificarResumen()
	{
        OnPropertyChanged(nameof(Canciones));
        AplicarFiltro();
        OnPropertyChanged(nameof(CancionesFiltradas));
        OnPropertyChanged(nameof(NumeroCanciones));
		OnPropertyChanged(nameof(NumeroCancionesFavoritas));
		OnPropertyChanged(nameof(NumeroArtistas));
    }

	/* TAREAS FILTRADAS */
	public ObservableCollection<Cancion> CancionesFiltradas { get; } = new();

	string textoFiltro = "";
	public string TextoFiltro
	{
		get => textoFiltro;
		set
		{
			if (textoFiltro == value) return;
			textoFiltro = value;
			OnPropertyChanged();
			AplicarFiltro();
	
		}
	}

	private void AplicarFiltro()
	{
		CancionesFiltradas.Clear();

		foreach (Cancion c in Canciones)
		{
			if(string.IsNullOrEmpty(TextoFiltro) || c.Titulo.Contains(TextoFiltro, StringComparison.OrdinalIgnoreCase))
			{
				CancionesFiltradas.Add(c);
			}
		}
	}

    /* JUGAR CON LAS CANCIONES */
    public async void AgregarCancion(Cancion cancion)
	{
        if (cancion == null) return;
        cancion.Codigo = "CANCION-" + id;
        id++;
        Canciones.Insert(0, cancion);
        await InsertarCancionBD(cancion);
        NotificarResumen();
		
	}

	public async void EliminarCancion(Cancion cancion)
	{
		if (cancion == null) return;
		if(Canciones.Remove(cancion))
		{
            await DeleteCancionBD(cancion);
			NotificarResumen();
		}
	}

	public async void MarcarComoFavorita(Cancion cancion)
	{
		if (cancion == null) return;
		if(!cancion.EsFavorita)
		{
			cancion.EsFavorita = true;
            await UpdateCancionBD(cancion);
			NotificarResumen();
		}
	}

	/* CAMBIAR LA ESCALA DE LAS CANCIONES */
	public void CambiarEscala(double escala)
	{
		foreach(var c in Canciones)
		{
			c.ScaleFactor = escala;
			c.PaddingElementos = paddingElementos * escala;
			c.MagnitudCorazon = magnitudCorazon * escala;
			c.MagnitudImagen = magnitudImagen * escala;
			c.PaddingElementos = paddingElementos * escala;
			c.TamanoTexto = tamanoTexto * escala;
			c.PaddingTarjeta = paddingTarjeta * escala;
		}
	}

    public string MensajeEstado
    {
        get => _mensajeEstado;
        set
        {
            if (_mensajeEstado == value) return;
            _mensajeEstado = value;
            OnPropertyChanged();
        }
    }

    /* SIMULADOR BOTON ESTÁ OCUPADO */
    public bool EstaOcupado
    {
        get => _estaOcupado;
        set
        {
            if (_estaOcupado == value) return;
            _estaOcupado = value;
            OnPropertyChanged();

            // Actualiza la disponibilidad del comando cuando cambia el estado
            (CargarCommand as Command)?.ChangeCanExecute();
        }
    }


    /* REGISTRO COMANDO */
    public ICommand CargarCommand { get; }

    /* CONECTA CON LA API REST DEVUELTA DESDE SPRING BOOT */
    private async Task SimularCargaAsync()
    {
        if (EstaOcupado) return;

        try
        {
            EstaOcupado = true;
            MensajeEstado = "Cargando...";

            Debug.WriteLine("Has intentado entrar");

            Canciones.Clear();
            NotificarResumen();
            await Task.Delay(800);

            var lista = await _service.GetCancionesAsync();

            Debug.WriteLine("Has hecho el servicio");

            foreach (var item in lista)
            {
                Canciones.Add(item);
            }

            MensajeEstado = $"Datos cargados {Canciones.Count}";
            NotificarResumen();
        }
        catch
        {
            MensajeEstado = "Error al cargar los datos";
        }
        finally
        {
            EstaOcupado = false;
        }
    }

    private async Task InsertarCancionBD (Cancion cancion)
    {
        await _service.PostCancionAsync(cancion);
    }

    private async Task UpdateCancionBD(Cancion cancion)
    {
        await _service.PatchCancionAsync(cancion);
    }

    private async Task DeleteCancionBD(Cancion cancion)
    {
        await _service.DeleteCancionAsync(cancion);
    }

}