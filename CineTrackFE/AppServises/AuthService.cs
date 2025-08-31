using CineTrackFE.Models;
using CineTrackFE.Models.DTO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CineTrackFE.AppServises;

public interface IAuthService
{
    Task<bool> LoginAsync(string email, string password, bool rememberMe = false);
    Task<bool> RegisterAsync(string email, string password);
    void Logout();
    bool IsAuthenticated { get; }
}


public class AuthService(HttpClient httpClient, UserStore userStore) : IAuthService
{
    private string? _token;
    private readonly HttpClient _httpClient = httpClient;
    private readonly UserStore _userStore = userStore;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);


    // LOGIN ASYNC //
    public async Task<bool> LoginAsync(string email, string password, bool rememberMe = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var loginDto = new { Email = email, Password = password, RememberMe = rememberMe };

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/AuthApi/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                _token = result?.Token;
                _userStore.User = result?.User;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                return true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while making the API request: {ex.Message}", ex);
        }
        return false;
    }


    // REGISTER ASYNC //
    public async Task<bool> RegisterAsync(string email, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/AuthApi/register", new { Email = email, Password = password}  );

            if (response.IsSuccessStatusCode) return true;
            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Registration failed: {error}");
            }

        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while making the API request: {ex.Message}", ex);
        }
        return false;
    }

    public void Logout()
    {
        _token = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public User User { get; set; } = default!;
}
