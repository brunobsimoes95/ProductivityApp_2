using System;

namespace ProdutividadeApp.Models
{

    // classe DTO (Data Transfer Object) só para transportar dados para poder fazer os gráficos.
    public class DailyChartData
    {
        public DateTime Date { get; set; }
        public int Value { get; set; }           // Dados de produtividade ou assim
        public string Emoji { get; set; } = "";  // Mood emoji
        public string Label { get; set; } = "";  //Dia da semana etc
        public string? Description { get; set; }

    }
}
