using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PRACTICA_RA2.Models
{
    public class Cancion : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string nombre = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));

        private string codigo;
        private string titulo;
        private List<string> artistas;
        private string duracion;
        private string urlPortada;
        private bool esFavorita = false;

        private double _scaleFactor = 1.0;
        private double magnitudImagen = 100;
        private double magnitudCorazon = 30;
        private double tamanoTexto = 16;
        private double paddingElementos = 8;
        private double paddingTarjeta = 14;

        public double ScaleFactor
        {
            get => _scaleFactor;
            set { _scaleFactor = value; OnPropertyChanged(); }
        }

        public string Codigo
        {
            get => codigo;
            set
            {
                codigo = value; OnPropertyChanged();
            }
        }

        public string Titulo
        {
            get => titulo;
            set
            {
                if (titulo == value) return;
                titulo = value;
                OnPropertyChanged();
            }
        }

        public List<string> Artistas
        {
            get => artistas;
            set
            {
                if (artistas == value) return;
                artistas = value;
                OnPropertyChanged();
            }
        }

        public string Duracion
        {
            get => duracion;
            set
            {
                if (duracion == value) return;
                duracion = value;
                OnPropertyChanged();
            }
        }

        public string UrlPortada
        {
            get => urlPortada;
            set
            {
                if (urlPortada == value) return;
                urlPortada = value;
                OnPropertyChanged();
            }
        }

        public bool EsFavorita
        {
            get => esFavorita;
            set
            {
                if (esFavorita == value) return;
                esFavorita = value;
                OnPropertyChanged();
            }
        }

        public string ArtistasString { get
            {
                string artistasA = string.Join(", ", artistas);
                return artistasA;
            }
        }

        public double MagnitudImagen
        {
            get => magnitudImagen; 
            set
            {
                magnitudImagen = value;
                OnPropertyChanged();
            }
        }

        public double TamanoTexto
        {
            get => tamanoTexto;
            set
            {
                tamanoTexto = value;
                OnPropertyChanged();
            }
        }

        public double PaddingElementos
        {
            get => paddingElementos;
            set
            {
                paddingElementos = value;
                OnPropertyChanged();
            }
        }

        public double PaddingTarjeta
        {
            get => paddingTarjeta;
            set
            {
                paddingTarjeta = value;
                OnPropertyChanged();
            }
        }

        public double MagnitudCorazon
        {
            get => magnitudCorazon;
            set
            {
                magnitudCorazon = value;
                OnPropertyChanged();
            }
        }

    }
}
