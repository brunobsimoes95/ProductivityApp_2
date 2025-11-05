using ProdutividadeApp.Models;
using SQLite;

namespace ProdutividadeApp.Services
{

    //Servico responsável pela gestão dos registos de humor (MoodEntry) na base de dados
    public class MoodService
    {
        private readonly SQLiteAsyncConnection _db;

        public MoodService(SQLiteAsyncConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _db.CreateTableAsync<MoodEntry>().Wait();
        }

        //// Método que devolve todos os moods da pessoa atualmente com sessão ativa
        public Task<List<MoodEntry>> GetAllAsync()
        {
            Console.WriteLine("💥 MoodService GetAllAsync()");
            if (_db == null)
            {
                Console.WriteLine("🚨 _db está NULL no MoodService!");
                return Task.FromResult(new List<MoodEntry>());
            }

            // Filtra só os registos da pessoa atual
            return _db.Table<MoodEntry>()
                      .Where(m => m.PessoaId == PessoaSessao.IdAtual)
                      .ToListAsync();
        }


        //Metodo que vai buscar pelo id
        public Task<MoodEntry?> GetByIdAsync(int id)
        {
            return _db.Table<MoodEntry>()
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync()!;
        }

        //Guarda
        public Task<int> SaveAsync(MoodEntry mood)
        {
            if (mood.Id != 0)
                return _db.UpdateAsync(mood);

            return _db.InsertAsync(mood);
        }

        public Task<int> DeleteAsync(MoodEntry mood)
        {
            return _db.DeleteAsync(mood);
        }
    }
}
