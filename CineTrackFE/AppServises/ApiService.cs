using System.Net.Http;
using System.Text.Json;

namespace CineTrackFE.AppServises
{
    public interface IApiService
    {
        Task<T?> GetAsync<T>(string endpoint);


    }



    public class ApiService(HttpClient httpClient) : IApiService
    {

        private readonly HttpClient _httpClient = httpClient;


        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(content);
                }
                else
                {
                    throw new HttpRequestException($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while making the API request: {ex.Message}", ex);
            }
        }
    }
}
