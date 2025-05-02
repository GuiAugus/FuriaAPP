using FuriaAPP.API.Data;
using FuriaAPP.API.Models;
using FuriaAPP.Shared.DTOs.Jogo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuriaAPP.API.Controllers
{
    [Route("api/competicoes/[controller]")]
    [ApiController]
    public class TemporadasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TemporadasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TemporadaDto>>> GetTemporadas()
        {
            var temporadas = await _context.Temporadas
                .Select(t => new TemporadaDto
                {
                    Id = t.Id,
                    CampeonatoId = t.CampeonatoId,
                    DataInicio = t.DataInicio,
                    DataFim = t.DataFim,
                    Tipo = t.Tipo
                })
                .ToListAsync();

            return Ok(temporadas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TemporadaDto>> GetTemporada(int id)
        {
            var temporada = await _context.Temporadas
                .Where(t => t.Id == id)
                .Select(t => new TemporadaDto
                {
                    Id = t.Id,
                    CampeonatoId = t.CampeonatoId,
                    DataInicio = t.DataInicio,
                    DataFim = t.DataFim,
                    Tipo = t.Tipo
                })
                .FirstOrDefaultAsync();

            if (temporada == null)
                return NotFound();

            return Ok(temporada);
        }

        [HttpPost]
        public async Task<ActionResult<TemporadaDto>> PostTemporada(TemporadaDto dto)
        {
            var campeonato = await _context.Campeonatos.FindAsync(dto.CampeonatoId);
            if (campeonato == null)
                return BadRequest("Campeonato não encontrado.");

            var temporada = new Temporada
            {
                CampeonatoId = dto.CampeonatoId,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                Tipo = dto.Tipo
            };

            _context.Temporadas.Add(temporada);
            await _context.SaveChangesAsync();

            dto.Id = temporada.Id;

            return CreatedAtAction(nameof(GetTemporada), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTemporada(int id, TemporadaDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var temporada = await _context.Temporadas.FindAsync(id);
            if (temporada == null)
                return NotFound();

            temporada.DataInicio = dto.DataInicio;
            temporada.DataFim = dto.DataFim;
            temporada.Tipo = dto.Tipo;
            temporada.CampeonatoId = dto.CampeonatoId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemporada(int id)
        {
            var temporada = await _context.Temporadas.FindAsync(id);
            if (temporada == null)
                return NotFound();

            _context.Temporadas.Remove(temporada);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
