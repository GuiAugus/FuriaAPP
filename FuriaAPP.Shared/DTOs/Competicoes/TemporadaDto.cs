// FuriaAPP.Shared/DTOs/Jogo/TemporadaDto.cs
namespace FuriaAPP.Shared.DTOs.Jogo
{
    public class TemporadaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int CampeonatoId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Tipo { get; set; } = string.Empty;
    }
}