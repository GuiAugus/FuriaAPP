namespace FuriaAPP.API.Models.Usuario
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;

        public ICollection<UsuarioJogoInteresse> JogosDeInteresse { get; set; } = new List<UsuarioJogoInteresse>();
    }
}
