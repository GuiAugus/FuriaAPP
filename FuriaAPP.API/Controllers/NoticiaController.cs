using FuriaAPP.API.Data;
using FuriaAPP.Shared.DTOs;
using FuriaAPP.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FuriaAPP.Shared.DTOs.Usuario;


namespace FuriaAPP.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<NoticiaController> _logger;

        public NoticiaController(AppDbContext context, ILogger<NoticiaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoticiaDto>>> GetAll(
            [FromQuery] List<int>? jogosInteresse = null)
        {
            try
            {
                var query = _context.Noticias
                    .Include(n => n.Jogo)
                    .AsQueryable();

                if (jogosInteresse != null && jogosInteresse.Any())
                {
                    query = query.Where(n => !n.JogoId.HasValue || jogosInteresse.Contains(n.JogoId.Value));
                }

                var noticias = await query
                    .Select(n => new NoticiaDto
                    {
                        Id = n.Id,
                        Titulo = n.Titulo,
                        Conteudo = n.Conteudo,
                        DataPublicacao = n.DataPublicacao,
                        JogoId = n.JogoId,
                        CampeonatoId = n.CampeonatoId,
                        TemporadaId = n.TemporadaId,
                        JogoHistoricoId = n.JogoHistoricoId,
                        NomeJogo = n.Jogo != null ? n.Jogo.Nome : null
                    })
                    .OrderByDescending(n => n.DataPublicacao)
                    .ToListAsync();

                return Ok(noticias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar notícias");
                return StatusCode(500, new { Message = "Erro interno ao processar a requisição" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoticiaDto>> GetById(int id)
        {
            var noticia = await _context.Noticias
                .Include(n => n.Jogo)
                .Include(n => n.Campeonato)
                .Include(n => n.Temporada)
                .Include(n => n.JogoHistorico)
                .Select(n => new NoticiaDto
                {
                    Id = n.Id,
                    Titulo = n.Titulo,
                    Conteudo = n.Conteudo,
                    DataPublicacao = n.DataPublicacao,
                    JogoId = n.JogoId,
                    CampeonatoId = n.CampeonatoId,
                    TemporadaId = n.TemporadaId,
                    JogoHistoricoId = n.JogoHistoricoId
                })
                .FirstOrDefaultAsync(n => n.Id == id);

            if (noticia == null)
            {
                return NotFound();
            }

            return Ok(noticia);
        }

        [HttpPost]
        public async Task<ActionResult<NoticiaDto>> Create(NoticiaDto dto)
        {
            var noticia = new Noticia
            {
                Titulo = dto.Titulo,
                Conteudo = dto.Conteudo,
                DataPublicacao = DateTime.UtcNow,
                JogoId = dto.JogoId,
                CampeonatoId = dto.CampeonatoId,
                TemporadaId = dto.TemporadaId,
                JogoHistoricoId = dto.JogoHistoricoId
            };

            _context.Noticias.Add(noticia);
            await _context.SaveChangesAsync();

            dto.Id = noticia.Id;
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NoticiaDto dto)
        {
            var noticia = await _context.Noticias.FindAsync(id);

            if (noticia == null)
            {
                return NotFound();
            }

            noticia.Titulo = dto.Titulo;
            noticia.Conteudo = dto.Conteudo;
            noticia.DataPublicacao = dto.DataPublicacao;

            if (dto.JogoId.HasValue)
            {
                noticia.JogoId = dto.JogoId;
            }
            if (dto.CampeonatoId.HasValue)
            {
                noticia.CampeonatoId = dto.CampeonatoId;
            }
            if (dto.TemporadaId.HasValue)
            {
                noticia.TemporadaId = dto.TemporadaId;
            }
            if (dto.JogoHistoricoId.HasValue)
            {
                noticia.JogoHistoricoId = dto.JogoHistoricoId;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var noticia = await _context.Noticias.FindAsync(id);

            if (noticia == null)
            {
                return NotFound();
            }

            _context.Noticias.Remove(noticia);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
