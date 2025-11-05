using ProdutividadeApp.Models;
//using ProdutividadeApp.Data;
using ProdutividadeApp.Services;

namespace ProdutividadeApp.Controllers
{
    // Controlador responsável por registar produtividade e pela lógica.
    public static class ProductivityController
    {
        public static async Task<ProductivityEntry> RegistarProdutividade(ProductivityService service, int nivel, string descricao)
        {
            var prod = new ProductivityEntry
            {
                NivelProdutividade = nivel,
                Descricao = descricao,
                Data = DateTime.Today,
                PessoaId = PessoaSessao.IdAtual
            };

            await service.SaveAsync(prod);
            return prod;
        }
    }

}
