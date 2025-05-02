using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuriaAPP.API.Models
{
    public class Campeonato
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [ForeignKey("Jogo")]
        public int JogoId { get; set; }
        public Jogo ?Jogo { get; set; }

        public ICollection<Temporada> Temporadas { get; set; } = new List<Temporada>();
    }
}
