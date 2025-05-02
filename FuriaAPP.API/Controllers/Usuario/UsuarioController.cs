using FuriaAPP.API.Data;
using FuriaAPP.API.Models.Usuario;
using FuriaAPP.Shared.DTOs.Usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuarioController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/usuario
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
    {
        var usuarios = await _context.Usuarios
            .Include(u => u.JogosDeInteresse) 
            .ThenInclude(ji => ji.Jogo) 
            .ToListAsync();

        if (usuarios == null || !usuarios.Any())
        {
            return Ok(new List<UsuarioDto>());
        }

        var usuariosDto = usuarios.Select(usuario => new UsuarioDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            CPF = usuario.CPF,
            Email = usuario.Email,
            JogoDeInteresse = usuario.JogosDeInteresse
                .Where(ji => ji.Jogo != null)
                .Select(ji => new UsuarioJogoInteresseDto
                {
                    JogoId = ji.JogoId,
                    NomeJogo = ji.Jogo.Nome
                })
                .ToList()
        }).ToList();

        return Ok(usuariosDto);
    }


    // GET: api/usuario/1
    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDto>> GetById(int id)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.JogosDeInteresse)
            .ThenInclude(ji => ji.Jogo) 
            .FirstOrDefaultAsync(u => u.Id == id);

        if (usuario == null)
        {
            return Ok(new UsuarioDto());
        }

        var usuarioDto = new UsuarioDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            CPF = usuario.CPF,
            Email = usuario.Email,
            JogoDeInteresse = usuario.JogosDeInteresse.Select(ji => new UsuarioJogoInteresseDto
            {
                JogoId = ji.JogoId,
                NomeJogo = ji.Jogo.Nome
            }).ToList()
        };

        return Ok(usuarioDto);
    }


    // POST: api/usuario/criar
    [HttpPost("cadastro")]
    public async Task<ActionResult<UsuarioDto>> Create(UsuarioDto usuarioDto)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == usuarioDto.Email))
        {
            return BadRequest(new { Message = "O email já está em uso." });
        }

        var usuario = new Usuario
        {
            Nome = usuarioDto.Nome,
            CPF = usuarioDto.CPF,
            Email = usuarioDto.Email,
            Senha = HashPassword(usuarioDto.Senha)
        };

        foreach (var jogo in usuarioDto.JogoDeInteresse ?? new List<UsuarioJogoInteresseDto>())
        {
            if (jogo?.JogoId != null)
            {
                var jogoExistente = await _context.Jogos.FindAsync(jogo.JogoId);
                if (jogoExistente != null)
                {
                    usuario.JogosDeInteresse.Add(new UsuarioJogoInteresse { JogoId = jogo.JogoId });
                }
                else
                {
                    return BadRequest(new { Message = $"Jogo com ID {jogo.JogoId} não encontrado." });
                }
            }
        }

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var usuarioDtoResponse = new UsuarioDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            CPF = usuario.CPF,
            Email = usuario.Email,
            JogoDeInteresse = usuario.JogosDeInteresse.Select(ji => new UsuarioJogoInteresseDto
            {
                JogoId = ji.JogoId,
                NomeJogo = ji.Jogo != null ? ji.Jogo.Nome : "Jogo não encontrado"  
            }).ToList()
        };

        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuarioDtoResponse);
    }



    // POST: api/usuario/login
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UsuarioLoginDto loginDto)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (usuario == null)
        {
            return Unauthorized("Credenciais inválidas.");
        }

        if (!VerifyPassword(loginDto.Senha, usuario.Senha))
        {
            return Unauthorized("Credenciais inválidas.");
        }

        return Ok("Login bem-sucedido!");
    }



    // DELETE: api/usuario/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        try
        {
            var usuario = await _context.Usuarios
                .Include(u => u.JogosDeInteresse) 
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound(new { Message = "Usuário não encontrado." });
            }

            _context.jogoJogoInteresse.RemoveRange(usuario.JogosDeInteresse);

            _context.Usuarios.Remove(usuario);

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Usuário deletado com sucesso." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erro ao deletar usuário.", Error = ex.Message });
        }
    }

    private string HashPassword(string senha)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(senha);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private bool VerifyPassword(string senhaDigitada, string senhaArmazenada)
    {
        var hashDigitado = HashPassword(senhaDigitada);
        return hashDigitado == senhaArmazenada;
    }
}
