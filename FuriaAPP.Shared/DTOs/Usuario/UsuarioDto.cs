namespace FuriaAPP.Shared.DTOs.Usuario
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;

        public List<UsuarioJogoInteresseDto> JogoDeInteresse { get; set; } = new List<UsuarioJogoInteresseDto>();
    }
}