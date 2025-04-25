namespace FuriaAPP.API.DTOs
{
    public class JogoHistoricoDto
    {
        public int Id { get; set; }

        public int JogoId { get; set; }
        public string Jogo { get; set; } = string.Empty;

        public int AdversarioId { get; set; }
        public string Adversario { get; set; } = string.Empty;

        public int CampeonatoId { get; set; }  
        public string Campeonato { get; set; } = string.Empty ;


        public DateTime DataJogo { get; set; }
        public string DataJogoFormatada => DataJogo.ToString("dd/MM/yyyy");
    }
}