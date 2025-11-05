using Microsoft.Extensions.Logging;         // Para logging/debug
using ProdutividadeApp.Services;            // Importa os serviços customizados
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore;
using Microsoft.Extensions.Configuration; // opcional mas útil
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection; // <- ESTE!
using Microsoft.Extensions.Http;


namespace ProdutividadeApp
{
    // Classe responsável por configurar e arrancar a aplicação MAUI
    public static class MauiProgram
    {
        // Método que cria e configura a app MAUI
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder(); // Cria o builder da app

            builder
                .UseMauiApp<App>()                 // Define a classe principal da app
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); // Adiciona fonte personalizada
                });

            builder.Services.AddMauiBlazorWebView(); // Permite usar Blazor WebView na app
             


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools(); // Ativa dev tools no modo DEBUG
            builder.Logging.AddDebug();                        // Ativa logging para debugging
#endif

            var connection = DbHelper.CreateConnection(); // Cria a ligação à base de dados SQLite
            

            // Regista a ligação e os serviços como singletons (1 instância única)
            builder.Services.AddSingleton(connection);
            builder.Services.AddSingleton<MoodService>(p => new MoodService(connection));
            builder.Services.AddSingleton<ProductivityService>(p => new ProductivityService(connection));
            builder.Services.AddSingleton<RegistryService>(p => new RegistryService(connection));
            builder.Services.AddSingleton<EmotionService>(p => new EmotionService(connection));
            builder.Services.AddSingleton<PessoaService>(p => new PessoaService(connection));
            builder.Services.AddSingleton<GoalService>(p => new GoalService(connection));
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.Services.AddHttpClient<CoachAI>();



            return builder.Build(); // Constrói e devolve a app configurada
        }
    }
}
