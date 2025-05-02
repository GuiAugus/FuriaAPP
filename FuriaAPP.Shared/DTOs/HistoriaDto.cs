namespace FuriaAPP.Shared.DTOs.Usuario
{
    public class HistoriaDto
    {
        public int Id { get; set;}
        public string Titulo { get; set;} = string.Empty;
        public string Conteudo { get; set;} = string.Empty;
        public string DataPublicacaoFormatada => DataPublicacao.ToString("dd/MM/yyyy");

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime DataPublicacao { get; set; }
    }
}