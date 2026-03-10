using Microsoft.Extensions.Logging;
using PRACTICA_RA2.ViewModels;
using PRACTICA_RA2.Pages;
using PRACTICA_RA2.Services;

namespace PRACTICA_RA2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //1 ViewModel universal
            builder.Services.AddSingleton<CancionesViewModel>();

            //Services Universal
            builder.Services.AddSingleton<HttpClient>(new HttpClient());
            builder.Services.AddSingleton<CancionesService>();

            //2 Paginas por instancia
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<DetalleCancionPage>();
            builder.Services.AddTransient<NuevaCancionPage>();
            builder.Services.AddTransient<InfoPage>();

            //3 AppShell universal
            builder.Services.AddSingleton<AppShell>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
