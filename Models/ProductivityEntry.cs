using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdutividadeApp.Models
{
    public class ProductivityEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int NivelProdutividade { get; set; }
        public string? Descricao { get; set; }
        public DateTime Data { get; set; } = DateTime.Today;

        public int PessoaId { get; set; }

    }
}
