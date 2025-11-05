using Microsoft.AspNetCore.Components;
using ProdutividadeApp.Models;
using ProdutividadeApp.Services;
using System.Globalization;

namespace ProdutividadeApp.Controllers
{

    //Controlador para o histórico de registos, processa e devolve os dados/historico de registos. Usado para graficos e listagens.
    public static class HistoryController
    {

        //Junta os dados dos serviços e devolve uma lista de registos completos para utilizador atual.
        public static async Task<List<RegistoCompleto>> GetHistoricoAsync(
           RegistryService registryService,
           MoodService moodService,
           ProductivityService prodService,
           EmotionService emotionService)
        {
            Console.WriteLine("🔍 HistoricoController iniciado.");

            if (PessoaSessao.IdAtual == 0)  //Verifica se utilizador
            {
                Console.WriteLine("⚠️ PessoaSessao.IdAtual está a 0!");
                return new List<RegistoCompleto>();
            }

            if (registryService == null || moodService == null || prodService == null || emotionService == null) //Verifica se os serviços estão a null
            {
                Console.WriteLine("❌ Um ou mais serviços estão NULL!");
                return new List<RegistoCompleto>();
            }

            var registries = await registryService.GetAllAsync();
            var moods = await moodService.GetAllAsync();
            var prods = await prodService.GetAllAsync();
            var emotions = await emotionService.GetAllAsync();

            Console.WriteLine($"📦 Dados carregados: Registries={registries.Count}, Moods={moods.Count}, Prods={prods.Count}, Emotions={emotions.Count}");

            //Linq para juntar os dados 
            var resultado = (from r in registries
                             where r.PessoaId == PessoaSessao.IdAtual
                             join m in moods on r.MoodEntryId equals m.Id into moodJoin
                             from m in moodJoin.DefaultIfEmpty()
                             join p in prods on r.ProductivityEntryId equals p.Id into prodJoin
                             from p in prodJoin.DefaultIfEmpty()
                             join e in emotions on m != null ? m.EmotionId : -1 equals e.Id into emoJoin
                             from e in emoJoin.DefaultIfEmpty()
                             select new RegistoCompleto
                             {
                                 Id = r.Id,
                                 Emoji = e?.Mood ?? "❓",
                                 MoodDescricao = m?.Descricao ?? "",
                                 ProdDescricao = p?.Descricao ?? "",
                                 Produtividade = p?.NivelProdutividade ?? 0,
                                 Data = m?.Data ?? DateTime.MinValue,
                                 MoodEntryId = m?.Id ?? 0,
                                 ProductivityEntryId = p?.Id ?? 0,
                                 SleepHours = r.SleepHours,
                                 StressLevel = r.StressLevel
                             }).ToList();

            Console.WriteLine($"✅ Histórico final com {resultado.Count} registos.");

            return resultado;
        }

        //Devolve os dados para o gráfico de produtividade semanal.
        public static async Task<List<DailyChartData>> GetWeeklyChartDataAsync(
            RegistryService registryService,
            MoodService moodService,
            ProductivityService prodService,
            EmotionService emotionService)
        {
            var registos = await GetHistoricoAsync(registryService, moodService, prodService, emotionService);
            var resultado = new List<DailyChartData>();

            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1); // segunda-feira
            var dias = Enumerable.Range(0, 7)
                .Select(i => startOfWeek.AddDays(i))
                .ToList();

            foreach (var d in dias)
            {
                var entrada = registos.FirstOrDefault(r => r.Data.Date == d.Date);
                resultado.Add(new DailyChartData
                {
                    Date = d,
                    Value = entrada?.Produtividade ?? 0,
                    Emoji = entrada?.Emoji ?? "",
                    Label = d.ToString("ddd", new CultureInfo("pt-PT"))
                });
            }

            return resultado;
        }


        //Metodo para top 3 humores semanais
        public static async Task<List<(string Emoji, int Count)>> GetTop3EmocoesSemanaAsync(
             RegistryService registryService,
             MoodService moodService,
             ProductivityService prodService,
             EmotionService emotionService)
        {
            var registos = await GetHistoricoAsync(registryService, moodService, prodService, emotionService);

            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);
            var endOfWeek = startOfWeek.AddDays(7);

            var emocoesSemana = registos
                .Where(r => r.Data >= startOfWeek && r.Data < endOfWeek)
                .Where(r => !string.IsNullOrWhiteSpace(r.Emoji))
                .GroupBy(r => r.Emoji)
                .Select(g => (Emoji: g.Key, Count: g.Count()))
                .OrderByDescending(g => g.Count)
                .Take(3)
                .ToList();

            return emocoesSemana;
        }



        //Metodo para top 3 humores totais
        //public static async Task<List<(string Emoji, int Count)>> GetTop3EmocoesAsync(
        //RegistryService registryService,
        //MoodService moodService,
        //ProductivityService prodService,
        //EmotionService emotionService)
        //{
        //    var historico = await GetHistoricoAsync(registryService, moodService, prodService, emotionService);

        //    var top3 = historico
        //        .GroupBy(r => r.Emoji)
        //        .Select(g => new { Emoji = g.Key, Count = g.Count() })
        //        .OrderByDescending(e => e.Count)
        //        .Take(3)
        //        .Select(e => (e.Emoji, e.Count))
        //        .ToList();

        //    return top3;
        //}

    }
}
