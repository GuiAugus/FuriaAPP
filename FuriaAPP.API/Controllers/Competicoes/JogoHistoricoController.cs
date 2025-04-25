using FuriaAPP.API.Data;
using FuriaAPP.API.DTOs;
using FuriaAPP.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuriaAPP.API.Controllers
{
    [Route("api/competicoes/[controller]")]
    [ApiController]
    public class JogoHistoricoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JogoHistoricoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoHistoricoDto>>> GetAll()
        {
            var jogosHistorico = await _context.jogoHistoricos
            .Select(j => new JogoHistoricoDto
            {
                Id = j.Id,
                Jogo = j.Jogo,
                Adversario = j.Adversario,
                Campeonato = j.Campeonato,
                DataJogo = j.DataJogo,
            })
            .ToListAsync();

            return Ok(jogosHistorico);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JogoHistoricoDto>> GetById(int id)
        {
            var jogosHistorico = await _context.jogoHistoricos
            .Where(j => j.Id == id)
            .Select(j => new JogoHistoricoDto
            {
                Id = j.Id,
                Adversario = j.Adversario,
                Campeonato = j.Campeonato,
                DataJogo = j.DataJogo
            })
            .FirstOrDefaultAsync();

            if (jogosHistorico == null)
            {
                return NotFound();
            }

            return Ok(jogosHistorico);
        }

        [HttpPost]
        public async Task<ActionResult<JogoHistoricoDto>> Create(JogoHistoricoDto dto)
        {
            var adversario = await _context.Adversarios.FindAsync(dto.AdversarioId);
            if (adversario == null)
            {
                return NotFound("Adversário não encontrado.");
            }

            var jogo = await _context.Jogos.FindAsync(dto.JogoId);
            if (jogo == null)
            {
                return NotFound("Jogo não encontrado.");
            }

            var campeonato = await _context.Campeonatos.FindAsync(dto.CampeonatoId);
            if (campeonato == null)
            {
                return NotFound("Campeonato não encontrado.");
            }

            var novoJogoHistorico = new JogoHistorico
            {
                Jogo = dto.Jogo,         
                Adversario = dto.Adversario,
                Campeonato = dto.Campeonato, 
                DataJogo = dto.DataJogo
            };

            _context.jogoHistoricos.Add(novoJogoHistorico);
            await _context.SaveChangesAsync();

            dto.Id = novoJogoHistorico.Id;

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, JogoHistoricoDto dto)
        {
            var jogoHistorico = await _context.jogoHistoricos.FindAsync(id);

            if (jogoHistorico == null)
            {
                return NotFound();
            }

            var jogo = await _context.Jogos.FindAsync(dto.Id);
            var adversario = await _context.Adversarios.FindAsync(dto.AdversarioId);
            var campeonato = await _context.Campeonatos.FindAsync(dto.CampeonatoId);

            if (jogo == null || adversario == null || campeonato == null)
            {
                return BadRequest ("Um ou mais IDs sao invalidos.");
            }

            jogoHistorico.Jogo = dto.Jogo;
            jogoHistorico.Adversario = dto.Adversario;
            jogoHistorico.Campeonato = dto.Campeonato;
            jogoHistorico.DataJogo = dto.DataJogo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var jogoHistorico = await _context.jogoHistoricos.FindAsync(id);

            if (jogoHistorico == null)
            {
                return NotFound();
            }

            _context.jogoHistoricos.Remove(jogoHistorico);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}