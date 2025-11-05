using Microsoft.AspNetCore.Components;

using ProdutividadeApp.Models;


namespace ProdutividadeApp.Controls
{
    public  class CheckLogin
    {
        // Verifica se o utilizador está autenticado, senão redireciona para o login
        public static int Verificar(NavigationManager nav)
        {
            if (PessoaSessao.IdAtual == 0)
            {
                nav.NavigateTo("/login");
            }
            return PessoaSessao.IdAtual;
        }

        
    }
}
