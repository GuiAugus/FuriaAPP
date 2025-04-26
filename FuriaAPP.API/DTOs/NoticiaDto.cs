public class NoticiaDto
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Conteudo { get; set; }
    public DateTime DataPublicacao { get; set; }

    public int? JogoId { get; set; }
    public int? CampeonatoId { get; set; }
    public int? TemporadaId { get; set; }
    public int? JogoHistoricoId { get; set; }
}