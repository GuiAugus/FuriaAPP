using Microsoft.AspNetCore.Mvc;
using FuriaAPP.API.Models;
using FuriaAPP.API.Data;
using FuriaAPP.Shared.DTOs.Usuario;

namespace FuriaAPP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HistoriaController(AppDbContext context)
        {
            _context = context;
        }
    

        [HttpGet]
        public ActionResult<IEnumerable<HistoriaDto>> GetAll()
        {
            var historias = _context.Historias
                .Select(h => new HistoriaDto
                {
                    Id = h.Id,
                    Titulo = h.Titulo,
                    Conteudo = h.Conteudo,
                    DataPublicacao = h.DataPublicacao,
                }).ToList();

            return Ok(historias);
        }

        [HttpGet("{id}")]
        public ActionResult<HistoriaDto> GetById(int id)
        {
            var historia = _context.Historias.Find(id);

            if (historia == null)
                return NotFound();

            var dto = new HistoriaDto
            {
                Id = historia.Id,
                Titulo = historia.Titulo,
                Conteudo = historia.Conteudo,
                DataPublicacao = historia.DataPublicacao,
            };

            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<HistoriaDto> Create(HistoriaDto dto)
        {
            var nova = new Historia
            {
                Titulo = dto.Titulo,
                Conteudo = dto.Conteudo,
                DataPublicacao = dto.DataPublicacao
            };

            _context.Historias.Add(nova);
            _context.SaveChanges();

            dto.Id = nova.Id;
            return CreatedAtAction(nameof(GetAll), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, HistoriaDto dto)
        {
            var historia = _context.Historias.Find(id);
            if (historia == null)
                return NotFound();

            historia.Titulo = dto.Titulo;
            historia.Conteudo = dto.Conteudo;
            historia.DataPublicacao = dto.DataPublicacao;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var historia = _context.Historias.Find(id);
            if (historia == null)
                return NotFound();

            _context.Historias.Remove(historia);
            _context.SaveChanges();

            return NoContent();
        }
    }
}