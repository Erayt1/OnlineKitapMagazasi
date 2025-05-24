using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OnlineKitapMagazasi.UI.Services;
using OnlineKitapMagazasi.UI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnlineKitapMagazasi.UI.Controllers
{
    public class KitapController : Controller
    {
        private readonly KitapService _kitapService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public KitapController(KitapService kitapService, HttpClient httpClient, IConfiguration config)
        {
            _kitapService = kitapService;
            _httpClient = httpClient;
            _config = config;
        }

        // kitapları listeliyorum, eğer arama varsa filtreliyorum
        public async Task<IActionResult> Index(string? arama)
        {
            var kitaplar = await _kitapService.TumKitaplariGetirAsync();

            if (!string.IsNullOrWhiteSpace(arama))
            {
                arama = arama.ToLower();
                kitaplar = kitaplar.Where(k =>
                    k.Baslik.ToLower().Contains(arama) ||
                    k.Yazar.ToLower().Contains(arama)
                ).ToList();
            }

            return View(kitaplar);
        }


        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Kitap kitap, IFormFile ResimDosyasi)
        {
            // resim varsa kaydediyorum
            if (ResimDosyasi != null && ResimDosyasi.Length > 0)
            {
                string dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(ResimDosyasi.FileName);
                string yol = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", dosyaAdi);

                using (var stream = new FileStream(yol, FileMode.Create))
                {
                    await ResimDosyasi.CopyToAsync(stream);
                }

                kitap.ResimUrl = "/images/" + dosyaAdi;
            }

            // token varsa onu alıp header'a ekliyorum
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                TempData["hata"] = "Yetkisiz işlem. Lütfen giriş yapın.";
                return RedirectToAction("Giris", "Auth");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // api'ye kitap bilgilerini json olarak gönderiyorum
            var kitapJson = JsonSerializer.Serialize(kitap);
            var content = new StringContent(kitapJson, Encoding.UTF8, "application/json");
            var apiUrl = _config["ApiSettings:BaseUrl"] + "kitap";
            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            TempData["hata"] = "Kitap eklenemedi. Yetkiniz olmayabilir.";
            return View(kitap);
        }



        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return Unauthorized();  // Giriş yapılmamışsa silme yetkisi yok

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var apiUrl = _config["ApiSettings:BaseUrl"] + "kitap/" + id;
            var response = await _httpClient.DeleteAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                TempData["hata"] = "Kitap silinemedi.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int id)
        {
            var apiUrl = _config["ApiSettings:BaseUrl"] + "kitap/" + id;
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var json = await response.Content.ReadAsStringAsync();
            var kitap = JsonSerializer.Deserialize<Kitap>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(kitap);
        }

        [HttpPost]
        public async Task<IActionResult> Duzenle(Kitap kitap, IFormFile? YeniResim)
        {
            if (YeniResim != null && YeniResim.Length > 0)
            {
                string dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(YeniResim.FileName);
                string yol = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", dosyaAdi);

                using (var stream = new FileStream(yol, FileMode.Create))
                {
                    await YeniResim.CopyToAsync(stream);
                }

                kitap.ResimUrl = "/images/" + dosyaAdi;
            }

            var json = JsonSerializer.Serialize(kitap);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var apiUrl = _config["ApiSettings:BaseUrl"] + "kitap/" + kitap.Id;
            var response = await _httpClient.PutAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(kitap);
        }





    }
}

