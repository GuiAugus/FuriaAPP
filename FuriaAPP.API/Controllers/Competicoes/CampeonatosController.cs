using FuriaAPP.API.Data;
using FuriaAPP.API.DTOs;
using FuriaAPP.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuriaAPP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampeonatosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CampeonatosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampeonatoDto>>> GetCampeonatos()
        {
            var campeonatos = await _context.Campeonatos
                .Select(c => new CampeonatoDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    DataInicio = c.DataInicio,
                    DataFim = c.DataFim,
                    Tipo = c.Tipo
                })
                .ToListAsync();

            return Ok(campeonatos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CampeonatoDto>> GetCampeonato(int id)
        {
            var campeonato = await _context.Campeonatos
                .Where(c => c.Id == id)
                .Select(c => new CampeonatoDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    DataInicio = c.DataInicio,
                    DataFim = c.DataFim,
                    Tipo = c.Tipo
                })
                .FirstOrDefaultAsync();

            if (campeonato == null)
            {
                return NotFound();
            }

            return Ok(campeonato);
        }

        [HttpPost]
        public async Task<ActionResult<CampeonatoDto>> PostCampeonato(CampeonatoDto dto)
        {
            var campeonato = new Campeonato
            {
                Nome = dto.Nome,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                Tipo = dto.Tipo
            };

            _context.Campeonatos.Add(campeonato);
            await _context.SaveChangesAsync();

            dto.Id = campeonato.Id;

            return CreatedAtAction(nameof(GetCampeonato), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampeonato(int id, CampeonatoDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var campeonato = await _context.Campeonatos.FindAsync(id);
            if (campeonato == null)
            {
                return NotFound();
            }

            campeonato.Nome = dto.Nome;
            campeonato.DataInicio = dto.DataInicio;
            campeonato.DataFim = dto.DataFim;
            campeonato.Tipo = dto.Tipo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampeonato(int id)
        {
            var campeonato = await _context.Campeonatos.FindAsync(id);
            if (campeonato == null)
            {
                return NotFound();
            }

            _context.Campeonatos.Remove(campeonato);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
