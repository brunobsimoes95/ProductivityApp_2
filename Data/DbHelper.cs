using SQLite;
using ProdutividadeApp.Models;
using ProdutividadeApp.Controls;
using ProdutividadeApp.Services;


namespace ProdutividadeApp.Services 
{
    public static class DbHelper
    {
        public static SQLiteAsyncConnection CreateConnection()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "produtividade.db3");
            return new SQLiteAsyncConnection(dbPath);
        }
    }
}
