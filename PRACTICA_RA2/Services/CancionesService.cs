using PRACTICA_RA2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace PRACTICA_RA2.Services
{
    public class CancionesService
    {

        private readonly HttpClient _http;
        private const string URL_Canciones = "http://127.0.0.1:8080/canciones";
        private const string URL_Cancion = "http://127.0.0.1:8080/cancion";
        private const string URL_PatchCancion = "http://127.0.0.1:8080/cancion/update";

        public CancionesService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Cancion>> GetCancionesAsync()
        {
            var lista = new List<Cancion>();
            try
            {
                var json = await _http.GetStringAsync(URL_Canciones);
                var opcionesSerializacion = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                lista = JsonSerializer.Deserialize<List<Cancion>>(json, opcionesSerializacion);

            } catch (Exception ex)
            {
                Debug.WriteLine($"Error de conexión: {ex.Message}");
            }
            return lista ?? new List<Cancion>();
        }

        public async Task PostCancionAsync(Cancion cancion)
        {
            try
            {
                var response = await _http.PostAsJsonAsync<Cancion>(URL_Cancion, cancion);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("¡Canción actualizada con éxito!");
                }
                else
                {
                    Debug.WriteLine($"Error de la API: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error de conexión: {ex.Message}");
            }
           
        }

        public async Task DeleteCancionAsync(Cancion cancion)
        {
            try
            {
                var response = await _http.DeleteAsync(URL_Cancion + "/" + cancion.Codigo);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("¡Canción actualizada con éxito!");
                }
                else
                {
                    Debug.WriteLine($"Error de la API: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error de conexión: {ex.Message}");
            }

        }

        public async Task PatchCancionAsync(Cancion cancion)
        {
            try
            {
                var response = await _http.PatchAsJsonAsync<Cancion>(URL_PatchCancion, cancion);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("¡Canción actualizada con éxito!");
                }
                else
                {
                    Debug.WriteLine($"Error de la API: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error de conexión: {ex.Message}");
            }

        }


    }
}
