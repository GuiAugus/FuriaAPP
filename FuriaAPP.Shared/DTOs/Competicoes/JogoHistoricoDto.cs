namespace FuriaAPP.Shared.DTOs.Jogo
{
    public class JogoHistoricoDto
    {
        public int Id { get; set; }

        public int JogoId { get; set; }
        public string Jogo { get; set; } = string.Empty;

        public int AdversarioId { get; set; }
        public string Adversario { get; set; } = string.Empty;

        public int PontuacaoFuria { get; set; }
        public int PontuacaoAdversario { get; set; }

        public string Resultado { get; set; } = string.Empty;

         public DateTime DataJogo { get; set; }
        public string DataJogoFormatada => DataJogo.ToString("dd/MM/yyyy");

        public int CampeonatoId { get; set; }  
        public string Campeonato { get; set; } = string.Empty ;

        public int TemporadaId { get; set; }
        public string Temporada { get; set; } = string.Empty;
    }
}