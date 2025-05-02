using Microsoft.AspNetCore.Mvc;
using FuriaAPP.API.Data;
using FuriaAPP.API.Models;
using FuriaAPP.Shared.DTOs.Jogo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FuriaAPP.API.Controllers
{
    [Route("api/competicoes/[controller]")]
    [ApiController]
    public class CampeonatosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CampeonatosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/competicoes/campeonatos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampeonatoDto>>> GetCampeonatos()
        {
            var campeonatos = await _context.Campeonatos
                .Include(c => c.Jogo)
                .Select(c => new CampeonatoDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    JogoId = c.JogoId,
                    JogoNome = c.Jogo!.Nome
                })
                .ToListAsync();

            return Ok(campeonatos);
        }

        // GET: api/competicoes/campeonatos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CampeonatoDetailsDto>> GetCampeonato(int id)
        {
            var campeonato = await _context.Campeonatos
                .Include(c => c.Jogo)
                .Include(c => c.Temporadas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campeonato == null)
            {
                return NotFound(new { Message = "Campeonato não encontrado" });
            }

            var result = new CampeonatoDetailsDto
            {
                Id = campeonato.Id,
                Nome = campeonato.Nome,
                JogoId = campeonato.JogoId,
                JogoNome = campeonato.Jogo!.Nome,
                Temporadas = campeonato.Temporadas.Select(t => new TemporadaDto
                {
                    Id = t.Id,
                    Nome = t.Nome,
                    DataInicio = t.DataInicio,
                    DataFim = t.DataFim,
                    Tipo = t.Tipo
                }).ToList()
            };

            return Ok(result);
        }

        // POST: api/competicoes/campeonatos
        [HttpPost]
        public async Task<ActionResult<CampeonatoDto>> PostCampeonato([FromBody] CampeonatoCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jogo = await _context.Jogos.FindAsync(dto.JogoId);
            if (jogo == null)
            {
                return BadRequest(new { Message = "Jogo não encontrado" });
            }

            var campeonatoExistente = await _context.Campeonatos
                .FirstOrDefaultAsync(c => c.Nome == dto.Nome && c.JogoId == dto.JogoId);
            
            if (campeonatoExistente != null)
            {
                return Conflict(new { Message = "Já existe um campeonato com este nome para este jogo" });
            }

            var campeonato = new Campeonato
            {
                Nome = dto.Nome.Trim(),
                JogoId = dto.JogoId
            };

            try
            {
                _context.Campeonatos.Add(campeonato);
                await _context.SaveChangesAsync();

                var result = new CampeonatoDto
                {
                    Id = campeonato.Id,
                    Nome = campeonato.Nome,
                    JogoId = campeonato.JogoId,
                    JogoNome = jogo.Nome
                };

                return CreatedAtAction(nameof(GetCampeonato), new { id = campeonato.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro ao criar campeonato", Error = ex.Message });
            }
        }

        // PUT: api/competicoes/campeonatos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampeonato(int id, [FromBody] CampeonatoUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { Message = "ID na URL não corresponde ao ID no corpo" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var campeonato = await _context.Campeonatos.FindAsync(id);
            if (campeonato == null)
            {
                return NotFound(new { Message = "Campeonato não encontrado" });
            }

            // Verifica se o jogo foi alterado
            if (campeonato.JogoId != dto.JogoId)
            {
                var novoJogo = await _context.Jogos.FindAsync(dto.JogoId);
                if (novoJogo == null)
                {
                    return BadRequest(new { Message = "Novo jogo não encontrado" });
                }
            }

            // Verifica se já existe outro campeonato com o novo nome para o mesmo jogo
            if (await _context.Campeonatos.AnyAsync(c => 
                c.Id != id && 
                c.Nome == dto.Nome && 
                c.JogoId == dto.JogoId))
            {
                return Conflict(new { Message = "Já existe um campeonato com este nome para este jogo" });
            }

            campeonato.Nome = dto.Nome.Trim();
            campeonato.JogoId = dto.JogoId;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampeonatoExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro ao atualizar campeonato", Error = ex.Message });
            }
        }

        // DELETE: api/competicoes/campeonatos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampeonato(int id)
        {
            var campeonato = await _context.Campeonatos
                .Include(c => c.Temporadas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campeonato == null)
            {
                return NotFound(new { Message = "Campeonato não encontrado" });
            }

            if (campeonato.Temporadas.Any())
            {
                return Conflict(new { 
                    Message = "Não é possível excluir o campeonato", 
                    Details = "Existem temporadas vinculadas a este campeonato",
                    TemporadasCount = campeonato.Temporadas.Count
                });
            }

            try
            {
                _context.Campeonatos.Remove(campeonato);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro ao excluir campeonato", Error = ex.Message });
            }
        }

        private bool CampeonatoExists(int id)
        {
            return _context.Campeonatos.Any(e => e.Id == id);
        }
    }
}