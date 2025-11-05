using ProdutividadeApp.Models;
using SQLite;

namespace ProdutividadeApp.Services
{
    /// Serviço responsável por interagir com a base de dados para os humores
    public class EmotionService
    {
        private readonly SQLiteAsyncConnection _db;

        /// Construtor que inicializa a conexão com a base de dados e cria a tabela de humores
        public EmotionService(SQLiteAsyncConnection db)
        {
            _db = db;
            _db.CreateTableAsync<Emotion>().Wait();
        }

        // Método que vai buscar todas as emoções da tabela
        public async Task<List<Emotion>> GetAllAsync()
        {
            return await _db.Table<Emotion>().ToListAsync();
        }

        // Método para preencher a base de dados com emoções padrão, se ainda estiver vazia
        public async Task SeedAsync()
        {
            var existing = await GetAllAsync();
            if (existing.Count == 0)
            {
                var moods = new List<Emotion>
                {
                    new Emotion { Mood = "😭 Muito Triste" },
                    new Emotion { Mood = "😔 Triste" },
                    new Emotion { Mood = "😐 Neutro" },
                    new Emotion { Mood = "🙂 Feliz" },
                    new Emotion { Mood = "😄 Muito Feliz" },
                    new Emotion { Mood = "🤩 Entusiasmado" },
                    new Emotion { Mood = "😤 Frustrado" },
                    new Emotion { Mood = "😴 Cansado" },
                    new Emotion { Mood = "😡 Irritado" },
                    new Emotion { Mood = "🧘‍♂️ Calmo" }
                };

                foreach (var mood in moods)
                    await _db.InsertAsync(mood);
            }
        }
    }
}
