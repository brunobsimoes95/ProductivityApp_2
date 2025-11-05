using ProdutividadeApp.Models;
using SQLite;

namespace ProdutividadeApp.Services
{

    // Serviço responsável por gerir a tabela de pessoas
    public class PessoaService
    {
        private readonly SQLiteAsyncConnection _db;

        // Método que insere um utilizador inicial se a tabela estiver vazia (usado para teste)
        //public async Task SeedAsync()
        //{
        //    var pessoas = await GetAllAsync();
        //    if (pessoas.Count == 0)
        //    {
        //        var novaPessoa = new Pessoa
        //        {
        //            Nome = "gemerilda",
        //            Email = "gemerilda@gmail.com",
        //            Password = "1234",
        //            Idade = 25,
        //            Peso = 70,
        //            Altura = 1.75f
        //        };

        //        await SaveAsync(novaPessoa);
        //        Console.WriteLine("? Utilizador inicial criado com sucesso.");
        //    }
        //}


        public PessoaService(SQLiteAsyncConnection db)
        {
            _db = db;
            _db.CreateTableAsync<Pessoa>().Wait();
        }

        public async Task<List<Pessoa>> GetAllAsync()
        {
            return await _db.Table<Pessoa>().ToListAsync();
        }

        public async Task<Pessoa?> GetByIdAsync(int id)
        {
            return await _db.Table<Pessoa>()
                             .Where(p => p.Id == id)
                             .FirstOrDefaultAsync();
        }

        public async Task<int> SaveAsync(Pessoa p)
        {
            if (p.Id != 0)
                return await _db.UpdateAsync(p);

            return await _db.InsertAsync(p);
        }

        public Task<int> DeleteAsync(Pessoa p)
        {
            return _db.DeleteAsync(p);
        }

        // Vai buscar uma pessoa pelo nome
        public async Task<Pessoa?> GetByNomeAsync(string nome)
        {
            return await _db.Table<Pessoa>()
                            .Where(p => p.Nome == nome)
                            .FirstOrDefaultAsync();
        }

    }
}
