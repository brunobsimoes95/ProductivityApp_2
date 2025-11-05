using ProdutividadeApp.Models;
using ProdutividadeApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdutividadeApp.Controllers
{
    // Controlador responsável pela lógica relacionada com as pessoas/utilizadores
    public static class PessoaController
    {
        // Método que faz o login de um utilizador com base no email e senha
        public static async Task<bool> LoginAsync(PessoaService service, string email, string senha)
        {
            var pessoas = await service.GetAllAsync();
            var pessoa = pessoas.FirstOrDefault(p => p.Email == email && p.Password == senha);

            if (pessoa != null)
            {
                PessoaSessao.IdAtual = pessoa.Id;
                Console.WriteLine($"✅ Login feito! IdAtual = {PessoaSessao.IdAtual}");
                return true;
            }
            else
            {
                Console.WriteLine("❌ Login falhou. Credenciais inválidas.");
                return false;
            }
        }
    }

}
