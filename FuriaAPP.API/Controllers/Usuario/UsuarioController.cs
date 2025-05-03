using FuriaAPP.API.Data;
using FuriaAPP.API.Models.Usuario;
using FuriaAPP.Shared.DTOs.Usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FuriaAPP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(
            AppDbContext context,
            IOptions<JwtSettings> jwtSettings,
            ILogger<UsuarioController> logger)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
        {
            try
            {
                var usuarios = await _context.Usuarios
                    .AsNoTracking()
                    .Include(u => u.JogosDeInteresse)
                    .ThenInclude(ji => ji.Jogo)
                    .OrderBy(u => u.Id)
                    .ToListAsync();

                return Ok(usuarios.Select(MapToDto).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os usuários");
                return StatusCode(500, new { Message = "Erro interno ao processar a requisição" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetById(int id)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out var userId))
                {
                    return Unauthorized(new { Message = "Usuário não autenticado corretamente" });
                }

                var usuario = await _context.Usuarios
                    .AsNoTracking()
                    .Include(u => u.JogosDeInteresse)
                    .ThenInclude(ji => ji.Jogo)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                {
                    return NotFound(new { Message = "Usuário não encontrado" });
                }

                if (userId != usuario.Id)
                {
                    return Forbid();
                }

                return Ok(MapToDto(usuario));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar usuário com ID {id}");
                return StatusCode(500, new { Message = "Erro interno ao processar a requisição" });
            }
        }

        [HttpPost("cadastro")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Cadastrar(UsuarioDto usuarioDto)
        {
            try
            {
                if (await _context.Usuarios.AnyAsync(u => u.Email == usuarioDto.Email))
                {
                    return BadRequest(new { Message = "Email já está em uso" });
                }

                var usuario = new Usuario
                {
                    Nome = usuarioDto.Nome,
                    CPF = usuarioDto.CPF,
                    Email = usuarioDto.Email,
                    Senha = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha + _jwtSettings.Pepper)
                };

                if (usuarioDto.JogoDeInteresse != null)
                {
                    foreach (var jogo in usuarioDto.JogoDeInteresse)
                    {
                        if (await _context.Jogos.AnyAsync(j => j.Id == jogo.JogoId))
                        {
                            usuario.JogosDeInteresse.Add(new UsuarioJogoInteresse { JogoId = jogo.JogoId });
                        }
                    }
                }

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(usuario);

                return Ok(new AuthResponse
                {
                    Token = token,
                    Usuario = MapToDto(usuario),
                    Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar usuário");
                return StatusCode(500, new { Message = "Erro interno ao processar o cadastro" });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Login(LoginDto loginDto)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .AsNoTracking()
                    .Include(u => u.JogosDeInteresse)
                    .ThenInclude(ji => ji.Jogo)
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

                if (usuario == null)
                {
                    _logger.LogWarning("Tentativa de login para email não cadastrado: {Email}", loginDto.Email);
                    return Unauthorized(new { Message = "Credenciais inválidas" });
                }

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Senha + _jwtSettings.Pepper, usuario.Senha))
                {
                    _logger.LogWarning("Senha incorreta para o usuário: {Email}", loginDto.Email);
                    return Unauthorized(new { Message = "Credenciais inválidas" });
                }

                var token = GenerateJwtToken(usuario);

                return Ok(new AuthResponse
                {
                    Token = token,
                    Usuario = MapToDto(usuario),
                    Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o login para {Email}", loginDto.Email);
                return StatusCode(500, new { Message = "Erro interno ao processar o login" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out var userId))
                {
                    return Unauthorized(new { Message = "Usuário não autenticado corretamente" });
                }

                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound(new { Message = "Usuário não encontrado" });
                }

                if (userId != usuario.Id)
                {
                    return Forbid();
                }

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar usuário com ID {Id}", id);
                return StatusCode(500, new { Message = "Erro interno ao processar a requisição" });
            }
        }

        #region Métodos Auxiliares
        private string GenerateJwtToken(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if (string.IsNullOrEmpty(_jwtSettings.SecretKey) || _jwtSettings.SecretKey.Length < 32)
                throw new ArgumentException("Configuração JWT inválida - chave secreta muito curta ou ausente");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim("jwt_version", "1.0") 
            };

            try
            {
                var jogosInteresse = usuario.JogosDeInteresse?
                    .Select(ji => new UsuarioJogoInteresseDto
                    {
                        JogoId = ji.JogoId,
                        NomeJogo = ji.Jogo?.Nome ?? "Desconhecido"
                    }).ToList() ?? new List<UsuarioJogoInteresseDto>();

                claims.Add(new Claim("jogos_interesse", 
                    JsonSerializer.Serialize(jogosInteresse, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    })));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao serializar jogos de interesse para JWT");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            try
            {
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar token JWT");
                throw new SecurityTokenException("Falha na geração do token de autenticação", ex);
            }
}

        private UsuarioDto MapToDto(Usuario usuario)
        {
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                CPF = usuario.CPF,
                Email = usuario.Email,
                JogoDeInteresse = usuario.JogosDeInteresse?
                    .Select(ji => new UsuarioJogoInteresseDto
                    {
                        JogoId = ji.JogoId,
                        NomeJogo = ji.Jogo?.Nome ?? "Jogo não encontrado"
                    }).ToList() ?? new List<UsuarioJogoInteresseDto>()
            };
        }
        #endregion
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public DateTime Expiration { get; set; }
    }
}