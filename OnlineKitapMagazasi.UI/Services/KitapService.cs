using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineKitapMagazasi.UI.Models;

namespace OnlineKitapMagazasi.UI.Services
{
    public class KitapService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public KitapService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<Kitap>> TumKitaplariGetirAsync()
        {
            string apiUrl = _config["ApiSettings:BaseUrl"] + "kitap";
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Kitap>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
