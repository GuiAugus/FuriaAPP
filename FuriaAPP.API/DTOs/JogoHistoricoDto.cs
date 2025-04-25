namespace FuriaAPP.API.DTOs
{
    public class JogoHistoricoDto
    {
        public int Id { get; set; }
        public string Jogo { get; set; } = string.Empty;
        public string Adversario { get; set; } = string.Empty;
        public string Campeonato { get; set; } = string.Empty ;
        public DateTime DataJogo { get; set; }

        public string DataJogoFormatada => DataJogo.ToString("dd/MM/yyyy");
    }
}