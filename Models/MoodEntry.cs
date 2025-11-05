using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ProdutividadeApp.Models
{
    public class MoodEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int EmotionId { get; set; }
        public string? Descricao { get; set; }
        public DateTime Data { get; set; } = DateTime.Today;
        public int PessoaId { get; set; }

    }
}
