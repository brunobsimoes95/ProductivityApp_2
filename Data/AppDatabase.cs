using ProdutividadeApp.Models;
using SQLite;

namespace ProdutividadeApp.Data
{
    public class AppDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public AppDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Pessoa>().Wait();
            _database.CreateTableAsync<Emotion>().Wait();
            _database.CreateTableAsync<MoodEntry>().Wait();
            _database.CreateTableAsync<ProductivityEntry>().Wait();
            _database.CreateTableAsync<Registry>().Wait();
        }

        public async Task SeedEmotionsAsync()
        {
            var existing = await _database.Table<Emotion>().ToListAsync();
            if (existing.Count == 0)
            {
                var moods = new List<Emotion>
                {
                    new Emotion { Mood = "😭 Muito Triste" },
                    new Emotion { Mood = "😔 Triste" },
                    new Emotion { Mood = "😐 Neutro" },
                    new Emotion { Mood = "🙂 Feliz" },
                    new Emotion { Mood = "😄 Muito Feliz" },
                    new Emotion { Mood = "🤩 Extasiado" },
                    new Emotion { Mood = "😤 Frustrado" },
                    new Emotion { Mood = "😴 Cansado" },
                    new Emotion { Mood = "😡 Irritado" },
                    new Emotion { Mood = "🧘‍♂️ Calmo" }
                };

                foreach (var mood in moods)
                {
                    await _database.InsertAsync(mood);
                }
            }
        }
    }
}