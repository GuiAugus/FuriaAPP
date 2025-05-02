using FuriaAPP.API.Data;
using FuriaAPP.API.Models.Usuario;
using FuriaAPP.Shared.DTOs.Usuario;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsuarioJogoInteresseController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuarioJogoInteresseController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("adicionar")]
    public async Task<IActionResult> AdicionarJogoDeInteresse(UsuarioJogoInteresseDto dto)
    {
        var usuario = await _context.Usuarios.FindAsync(dto.JogoId);
        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        var jogoExistente = await _context.Jogos.FindAsync(dto.JogoId);
        if (jogoExistente == null)
        {
            return BadRequest($"Jogo com ID {dto.JogoId} não encontrado.");
        }

        var interesseExistente = usuario.JogosDeInteresse
            .FirstOrDefault(j => j.JogoId == dto.JogoId);
        
        if (interesseExistente != null)
        {
            return BadRequest("Este jogo já está na lista de interesses do usuário.");
        }

        var interesse = new UsuarioJogoInteresse
        {
            UsuarioId = dto.JogoId,
            JogoId = dto.JogoId
        };

        usuario.JogosDeInteresse.Add(interesse);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Jogo de interesse adicionado com sucesso." });
    }
}