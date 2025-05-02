using System.ComponentModel.DataAnnotations;

namespace FuriaAPP.Shared.DTOs.Jogo
{
    public class CampeonatoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int JogoId { get; set; }
        public string JogoNome { get; set; } = string.Empty; // Adicionado para facilitar o frontend
    }

    public class CampeonatoCreateDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Máximo de 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "JogoId é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "JogoId inválido")]
        public int JogoId { get; set; }
    }

    public class CampeonatoUpdateDto
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Máximo de 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "JogoId é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "JogoId inválido")]
        public int JogoId { get; set; }
    }

    public class CampeonatoDetailsDto : CampeonatoDto
    {
        public List<TemporadaDto> Temporadas { get; set; } = new();
    }
}