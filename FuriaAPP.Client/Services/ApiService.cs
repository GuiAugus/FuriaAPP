using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FuriaAPP.Shared.DTOs.Usuario;

namespace FuriaAPP.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UsuarioDto>> GetUsuariosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UsuarioDto>>("https://localhost:5101/api/usuarios");
        }

        public async Task<UsuarioDto> GetUsuarioByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<UsuarioDto>($"https://localhost:5101/api/usuarios/{id}");
        }

        public async Task<UsuarioDto> CreateUsuarioAsync(UsuarioDto usuarioDto)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:5101/api/usuarios", usuarioDto);
            return await response.Content.ReadFromJsonAsync<UsuarioDto>();
        }
    }
}