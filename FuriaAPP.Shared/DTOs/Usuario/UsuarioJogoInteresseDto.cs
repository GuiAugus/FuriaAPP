using System.Text.Json.Serialization;

namespace FuriaAPP.Shared.DTOs.Usuario
{
    public class UsuarioJogoInteresseDto
    {
        [JsonPropertyName("jogoId")]
        public int JogoId { get; set; }

        [JsonPropertyName("nomeJogo")]
        public string NomeJogo { get; set; } = string.Empty;
    }
}