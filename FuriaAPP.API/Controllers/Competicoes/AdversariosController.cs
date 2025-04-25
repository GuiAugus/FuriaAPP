using FuriaAPP.API.Data;
using FuriaAPP.API.DTOs;
using FuriaAPP.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuriaAPP.API.Controllers
{
    [Route("api/competicoes/[controller]")]
    [ApiController]
    public class AdversariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdversariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdversarioDto>>> GetAdversarios()
        {
            var adversarios = await _context.Adversarios
            .Select(a => new AdversarioDto
            {
                Id = a.Id,
                Nome = a.Nome,
            })
            .ToListAsync();

            return Ok(adversarios);
        
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdversarioDto>> GetAdversario(int id)
        {
            var adversario = await _context.Adversarios
            .Where(a => a.Id == id)
            .Select(a => new AdversarioDto
            {
                Id = a.Id,
                Nome = a.Nome,
            })
            .FirstOrDefaultAsync();

            if (adversario == null)
            {
                return NotFound();
            }

            return Ok(adversario);
        }

        [HttpPost]
        public async Task<ActionResult<AdversarioDto>> PostAdversario(AdversarioDto dto)
        {
            var adversario = new Adversario
            {
                Nome = dto.Nome,
            };

            _context.Adversarios.Add(adversario);
            await _context.SaveChangesAsync();

            dto.Id = adversario.Id;

            return CreatedAtAction(nameof(GetAdversario), new { id = dto.Id}, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdversario(int id, AdversarioDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var adversario = await _context.Adversarios.FindAsync(id);
            if (adversario == null)
            {
                return NotFound();
            }

            adversario.Nome = dto.Nome;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdversario(int id)
        {
            var adversario = await _context.Adversarios.FindAsync(id);
            if (adversario == null)
            {
                return NotFound();
            }

            _context.Adversarios.Remove(adversario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}