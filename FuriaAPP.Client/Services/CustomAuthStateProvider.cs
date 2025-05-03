using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using FuriaAPP.Shared.DTOs.Usuario;
using Microsoft.Extensions.Logging;

public class CustomAuthStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;
    private readonly ILogger<CustomAuthStateProvider> _logger;
    private bool _disposed;
    private readonly AuthenticationState _anonymous;

    public CustomAuthStateProvider(
        ILocalStorageService localStorage,
        HttpClient http,
        ILogger<CustomAuthStateProvider> logger)
    {
        _localStorage = localStorage;
        _http = http;
        _logger = logger;
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(token))
                return _anonymous;

            if (IsTokenExpired(token))
            {
                await ClearToken();
                return _anonymous;
            }

            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);
            
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            return new AuthenticationState(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estado de autenticação");
            await ClearToken();
            return _anonymous;
        }
    }

    public async Task NotifyUserAuthentication(string token, UsuarioDto usuario)
    {
        try
        {
            await _localStorage.SetItemAsync("authToken", token);
            await _localStorage.SetItemAsync("usuario", usuario);
            
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(user);
            
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao notificar autenticação do usuário");
            await ClearToken();
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
        }
    }

    public async Task NotifyUserLogout()
    {
        await ClearToken();
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
    }

    private bool IsTokenExpired(string token)
    {
        try
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwt.ValidTo < DateTime.UtcNow.AddMinutes(1); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar expiração do token");
            return true;
        }
    }

    private async Task ClearToken()
    {
        try
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("usuario");
            _http.DefaultRequestHeaders.Authorization = null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao limpar token");
        }
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        if (string.IsNullOrWhiteSpace(jwt))
            return Enumerable.Empty<Claim>();

        try
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(jwt))
                return Enumerable.Empty<Claim>();

            return handler.ReadJwtToken(jwt).Claims;
        }
        catch
        {
            return Enumerable.Empty<Claim>();
        }
    }

    public async Task<UsuarioDto?> GetUsuarioLogado()
    {
        try
        {
            return await _localStorage.GetItemAsync<UsuarioDto>("usuario");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter usuário logado");
            return null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
            }
            _disposed = true;
        }
    }
}