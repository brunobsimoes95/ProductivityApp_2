using SQLite;
using System.ComponentModel.DataAnnotations;

namespace ProdutividadeApp.Models
{
    public class Pessoa
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required]
        public string? Nome { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public int Idade { get; set; }
        public float Peso { get; set; }
        public float Altura { get; set; }
    }
}
