using FuriaAPP.API.Models;

public class Noticia
{
    public int Id { get; set; }
    public string? Titulo { get; set; } = string.Empty;
    public string? Conteudo { get; set; } = string.Empty;
    public DateTime DataPublicacao { get; set; }

    public int? JogoId { get; set; }
    public Jogo? Jogo { get; set; } 

    public int? CampeonatoId { get; set; }
    public Campeonato? Campeonato{ get; set; }

    public int? TemporadaId { get; set; }
    public Temporada? Temporada{ get; set; }

    public int? JogoHistoricoId { get; set; }
    public JogoHistorico? JogoHistorico{ get; set; }
}