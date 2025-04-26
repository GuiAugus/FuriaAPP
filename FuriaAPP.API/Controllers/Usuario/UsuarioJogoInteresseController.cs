using FuriaAPP.API.Data;
using FuriaAPP.API.DTOs;
using FuriaAPP.API.DTOs.Usuario;
using FuriaAPP.API.Models.Usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UsuarioJogoInteresseController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuarioJogoInteresseController(AppDbContext context)
    {
        _context = context;
    }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.JogosDeInteresse)
                .ThenInclude(ji => ji.Jogo)
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    CPF = u.CPF,
                    Email = u.Email,
                    JogoDeInteresse = u.JogosDeInteresse.Select(ji => new UsuarioJogoInteresseDto
                    {
                        JogoId = ji.JogoId,
                        NomeJogo = ji.Jogo.Nome
                    }).ToList()
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetById(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.JogosDeInteresse)
                .ThenInclude(ji => ji.Jogo)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            var usuarioDtoResponse = new UsuarioDto
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

            return Ok(usuarioDtoResponse);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> Create(UsuarioDto usuarioDto)
        {
            var usuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                CPF = usuarioDto.CPF,
                Email = usuarioDto.Email,
                Senha = usuarioDto.Senha
            };

            foreach (var jogo in usuarioDto.JogoDeInteresse ?? new List<UsuarioJogoInteresseDto>())
            {
                usuario.JogosDeInteresse.Add(new UsuarioJogoInteresse { JogoId = jogo.JogoId });
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
                    NomeJogo = ji.Jogo.Nome
                }).ToList()
            };

            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuarioDtoResponse);
        }


        [HttpPost("{usuarioId}/interesse/{jogoId}")]
        public async Task<IActionResult> AddJogoInteresse(int usuarioId, int jogoId)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.JogosDeInteresse)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
            {
                return NotFound();
            }

            if (usuario.JogosDeInteresse.Any(ji => ji.JogoId == jogoId))
            {
                return BadRequest("O jogo já está nos interesses do usuário.");
            }

            usuario.JogosDeInteresse.Add(new UsuarioJogoInteresse { JogoId = jogoId });

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UsuarioDto usuarioDto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.JogosDeInteresse)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Nome = usuarioDto.Nome;
            usuario.CPF = usuarioDto.CPF;
            usuario.Email = usuarioDto.Email;
            usuario.Senha = usuarioDto.Senha;

            usuario.JogosDeInteresse.Clear();
            foreach (var jogo in usuarioDto.JogoDeInteresse ?? new List<UsuarioJogoInteresseDto>())
            {
                usuario.JogosDeInteresse.Add(new UsuarioJogoInteresse { JogoId = jogo.JogoId });
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
}
