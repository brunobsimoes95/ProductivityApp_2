using ProdutividadeApp.Models;
using ProdutividadeApp.Services;

namespace ProdutividadeApp.Controllers
{
    // Controlador responsável por registar os moods 
    public static class MoodController
    {
        // Método assíncrono que cria e guarda um novo MoodEntry, registar humor
        public static async Task<MoodEntry> RegistarMood(MoodService service, int emotionId, string descricao)
        {
            var mood = new MoodEntry
            {
                EmotionId = emotionId,
                Descricao = descricao,
                Data = DateTime.Today,
                PessoaId = PessoaSessao.IdAtual
            };

            await service.SaveAsync(mood);
            return mood;
        }
    }
}
