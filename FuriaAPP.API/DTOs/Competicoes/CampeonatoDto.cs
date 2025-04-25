namespace FuriaAPP.API.DTOs
{
    public class CampeonatoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Tipo { get; set; } = string.Empty;
    }
}
