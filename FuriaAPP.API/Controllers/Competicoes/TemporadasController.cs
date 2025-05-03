using FuriaAPP.API.Data;
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
                .Include(t => t.Campeonato)
                .Select(t => new FuriaAPP.Shared.DTOs.Jogo.TemporadaDto
                {
                    Id = t.Id,
                    Nome = t.Nome,
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
                .Include(t => t.Campeonato)
                .Where(t => t.Id == id)
                .Select(t => new FuriaAPP.Shared.DTOs.Jogo.TemporadaDto
                {
                    Id = t.Id,
                    Nome = t.Nome,
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool campeonatoExiste = await _context.Campeonatos.AnyAsync(c => c.Id == dto.CampeonatoId);
            if (!campeonatoExiste)
                return BadRequest("Campeonato não encontrado.");

            if (dto.DataFim.HasValue && dto.DataFim.Value < dto.DataInicio)
                return BadRequest("Data de fim não pode ser anterior à data de início");

            var temporada = new Temporada
            {
                Nome = dto.Nome,
                CampeonatoId = dto.CampeonatoId,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                Tipo = dto.Tipo
            };

            try
            {
                _context.Temporadas.Add(temporada);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Erro ao salvar temporada : {ex.InnerException?.Message}");
                return BadRequest("Erro ao criar a temporada. verifique os dados");
            }

            dto.Id = temporada.Id;
            return CreatedAtAction(nameof(GetTemporada), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTemporada(int id, TemporadaDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não corresponde ao ID do corpo");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var temporada = await _context.Temporadas.FindAsync(id);
            if (temporada == null)
                return NotFound();

            var campeonato = await _context.Campeonatos.FindAsync(dto.CampeonatoId);
            if (campeonato == null)
                return BadRequest("Campeonato não encontrado.");

            if (dto.DataFim.HasValue && dto.DataFim.Value < dto.DataInicio)
                return BadRequest("Data de fim não pode ser anterior à data de início");

            temporada.Nome = dto.Nome;
            temporada.CampeonatoId = dto.CampeonatoId;
            temporada.DataInicio = dto.DataInicio;
            temporada.DataFim = dto.DataFim;
            temporada.Tipo = dto.Tipo;

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