using FuriaAPP.API.Models;

public class Temporada
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int CampeonatoId { get; set; }
    public Campeonato Campeonato { get; set; } = new Campeonato();
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string Tipo { get; set; } = string.Empty;
}
