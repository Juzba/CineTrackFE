using CineTrackFE.Models.DTO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CineTrackFE.AppServises
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password, bool rememberMe = false);
        void Logout();
        bool IsAuthenticated { get; }
    }

    public class AuthService : IAuthService
    {
        private string? _token;
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(_token);

        public async Task<bool> LoginAsync(string username, string password, bool rememberMe = false)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(username);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            var loginDto = new LoginDto() { UserName = username, Password = password, RememberMe = rememberMe };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/AuthApi/login", loginDto);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    _token = result?.Token;

                    // Přidejte token do hlavičky pro všechny následující requesty
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _token);

                    return true;
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
    }
}
