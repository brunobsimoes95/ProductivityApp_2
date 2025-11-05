using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdutividadeApp.Models
{
    public class RegistoCompleto
    {
        public int Id { get; set; }
        public string Emoji { get; set; } = "";
        public string MoodDescricao { get; set; } = "";
        public string ProdDescricao { get; set; } = "";
        public int Produtividade { get; set; }
        public DateTime Data { get; set; } = DateTime.Today;

        public int MoodEntryId { get; set; }
        public int ProductivityEntryId { get; set; }

        public int SleepHours { get; set; }
        public int StressLevel { get; set; }
    }
}

