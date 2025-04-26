using FuriaAPP.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuriaAPP.API.Models.Usuario
{
    public class UsuarioJogoInteresse
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public int JogoId { get; set; }
        public Jogo Jogo { get; set; } = null!;
    }
}
