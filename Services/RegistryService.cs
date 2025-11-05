using ProdutividadeApp.Models;
using SQLite;

namespace ProdutividadeApp.Services
{

    //servico responsável pelos registos (Registry) na base de dados
    public class RegistryService
    {
        private readonly SQLiteAsyncConnection _db;

        public RegistryService(SQLiteAsyncConnection db)
        {
            _db = db;
            _db.CreateTableAsync<Registry>().Wait();
        }

        
        public async Task<List<Registry>> GetAllAsync()
        {
            return await _db.Table<Registry>()
                            .Where(r => r.PessoaId == PessoaSessao.IdAtual)
                            .ToListAsync();
        }

        public async Task<int> SaveAsync(Registry entry)
        {
            if (entry.Id != 0)
                return await _db.UpdateAsync(entry);

            return await _db.InsertAsync(entry);
        }

        public Task<int> DeleteAsync(Registry entry)
        {
            return _db.DeleteAsync(entry);
        }

        public async Task<Registry> GetByIdAsync(int id)
        {
            return await _db.Table<Registry>().FirstOrDefaultAsync(r => r.Id == id);
        }

    }
}
