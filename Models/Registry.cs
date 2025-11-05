using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdutividadeApp.Models
{
    //
    public class Registry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int MoodEntryId { get; set; }
        public int ProductivityEntryId { get; set; }
        public int PessoaId { get; set; }

        public DateTime Data { get; set; } = DateTime.Today;

        public int SleepHours { get; set; }
        public int StressLevel { get; set; }


    }


}
