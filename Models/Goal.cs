using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdutividadeApp.Models
{
    public enum GoalType
    {
        Produtividade,
        Humor,
        Consistencia,
        Personalizado
    }

    public class Goal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public GoalType Tipo { get; set; }

        public string Titulo { get; set; }  // Ex: "Alcançar 80% de produtividade"
        public string Descricao { get; set; } // Explicação opcional

        public double? ProductivityTarget { get; set; } // Usado só se for produtividade
        public string? EmojiAlvo { get; set; } // Para objetivos de humor
        public int? DiasConsecutivos { get; set; } // Para streaks ou consistência

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }
    }



}
