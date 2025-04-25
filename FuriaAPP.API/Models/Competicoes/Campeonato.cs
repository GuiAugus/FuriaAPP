using System;
using System.ComponentModel.DataAnnotations;

namespace FuriaAPP.API.Models
{
    public class Campeonato
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        [Required]
        public string Tipo { get; set; } = string.Empty; 
    }
}
