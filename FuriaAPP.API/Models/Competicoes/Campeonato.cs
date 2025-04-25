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

        public ICollection<Temporada> Temporadas { get; set; } = new List<Temporada>();
    }
}
