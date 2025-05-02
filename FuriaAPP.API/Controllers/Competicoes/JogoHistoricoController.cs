using FuriaAPP.API.Data;
using FuriaAPP.API.Models;
using FuriaAPP.Shared.DTOs.Jogo;
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
            .Include(j => j.Temporada)
            .ThenInclude(t => t.Campeonato)
            .Select(j => new JogoHistoricoDto
            {
                Id = j.Id,
                Jogo = j.Jogo,
                Adversario = j.Adversario,
                Campeonato = j.Campeonato,
                DataJogo = j.DataJogo,
                Temporada = j.Temporada.Nome,
                Resultado = j.PontuacaoFuria > j.PontuacaoAdversario ? "Vitória"
                      : j.PontuacaoFuria < j.PontuacaoAdversario ? "Derrota"
                      : "Empate"
            })
            .ToListAsync();

            return Ok(jogosHistorico);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JogoHistoricoDto>> GetById(int id)
        {
            var jogosHistorico = await _context.jogoHistoricos
            .Include(j => j.Temporada)
            .ThenInclude(t => t.Campeonato)
            .Select(j => new JogoHistoricoDto
            {
                Id = j.Id,
                Jogo = j.Jogo,
                Adversario = j.Adversario,
                Campeonato = j.Campeonato,
                DataJogo = j.DataJogo,
                Temporada = j.Temporada.Nome,
                Resultado = j.PontuacaoFuria > j.PontuacaoAdversario ? "Vitória"
                      : j.PontuacaoFuria < j.PontuacaoAdversario ? "Derrota"
                      : "Empate"
            })
            .FirstOrDefaultAsync(j => j.Id == id);

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

            var temporada = await _context.Temporadas.FindAsync(dto.TemporadaId);
            if (temporada == null)
            {
                return NotFound("Temporada não encontrada.");
            }

            var novoJogoHistorico = new JogoHistorico
            {
                Jogo = dto.Jogo,         
                Adversario = dto.Adversario,
                Temporada = temporada,
                CampeonatoId = temporada.CampeonatoId, 
                DataJogo = dto.DataJogo,
                PontuacaoFuria = dto.PontuacaoFuria,
                PontuacaoAdversario = dto.PontuacaoAdversario,
                Resultado = dto.PontuacaoFuria > dto.PontuacaoAdversario ? "Vitória"
                        : dto.PontuacaoFuria < dto.PontuacaoAdversario ? "Derrota"
                        : "Empate"
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
            var temporada = await _context.Temporadas.FindAsync(dto.TemporadaId);

            if (jogo == null || adversario == null || temporada == null)
            {
                return BadRequest ("Um ou mais IDs sao invalidos.");
            }

            jogoHistorico.Jogo = dto.Jogo;
            jogoHistorico.Adversario = dto.Adversario;
            jogoHistorico.CampeonatoId = temporada.CampeonatoId;
            jogoHistorico.Temporada = temporada;
            jogoHistorico.DataJogo = dto.DataJogo;
            jogoHistorico.PontuacaoFuria = dto.PontuacaoFuria;
            jogoHistorico.PontuacaoAdversario = dto.PontuacaoAdversario;
            jogoHistorico.Resultado = dto.PontuacaoFuria > dto.PontuacaoAdversario ? "Vitória"
                        : dto.PontuacaoFuria < dto.PontuacaoAdversario ? "Derrota"
                        : "Empate";

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