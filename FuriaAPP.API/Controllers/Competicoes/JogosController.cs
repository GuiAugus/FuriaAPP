using Microsoft.AspNetCore.Mvc;
using FuriaAPP.API.Data;
using FuriaAPP.API.DTOs;
using FuriaAPP.API.Models;

namespace FuriaAPP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JogosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<JogoDto>> GetJogos()
        {
            var jogos = _context.Jogos
                .Select(j => new JogoDto
                {
                    Id = j.Id,
                    Nome = j.Nome,
                }).ToList();

            return Ok(jogos);
        }

        [HttpGet("{id}")]
        public ActionResult<JogoDto> GetJogo(int id)
        {
            var jogo = _context.Jogos
                .Where(j => j.Id == id)
                .Select(j => new JogoDto
                {
                    Id = j.Id,
                    Nome = j.Nome,
                }).FirstOrDefault();

            if (jogo == null)
            {
                return NotFound();
            }

            return Ok(jogo);
        }

        [HttpPost]
        public ActionResult<JogoDto> PostJogo(JogoDto dto)
        {
            var jogo = new Jogo
            {
                Nome = dto.Nome,
            };

            _context.Jogos.Add(jogo);
            _context.SaveChanges();

            dto.Id = jogo.Id;

            return CreatedAtAction(nameof(GetJogo), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public ActionResult PutJogo(int id, JogoDto dto)
        {
            var jogo = _context.Jogos.Find(id);
            if (jogo == null)
            {
                return NotFound();
            }

            jogo.Nome = dto.Nome;

            _context.Jogos.Update(jogo);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteJogo(int id)
        {
            var jogo = _context.Jogos.Find(id);
            if (jogo == null)
            {
                return NotFound();
            }

            _context.Jogos.Remove(jogo);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
