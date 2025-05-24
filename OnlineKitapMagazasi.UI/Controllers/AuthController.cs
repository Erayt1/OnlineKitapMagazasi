using Microsoft.AspNetCore.Mvc;
using OnlineKitapMagazasi.UI.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace OnlineKitapMagazasi.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AuthController(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        [HttpGet]
        public IActionResult Kayit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Kayit(Kullanici model)
        {
            var url = _config["ApiSettings:BaseUrl"] + "auth/kayit";
            var response = await _httpClient.PostAsJsonAsync(url, model);

            if (response.IsSuccessStatusCode)
            {
                TempData["mesaj"] = "Kayıt başarılı, giriş yapabilirsiniz.";
                return RedirectToAction("Giris");
            }

            ModelState.AddModelError(string.Empty, "Kayıt başarısız.");
            return View(model);
        }
            
        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Giris(Kullanici model)
        {
            var url = _config["ApiSettings:BaseUrl"] + "auth/giris";
            var response = await _httpClient.PostAsJsonAsync(url, model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Giriş başarısız.");
                return View(model);
            }

            var veri = await response.Content.ReadFromJsonAsync<TokenCevap>();
            HttpContext.Session.SetString("token", veri.Token); //  burada token saklanıyor

            return RedirectToAction("Index", "Kitap");
        }


        public class TokenCevap
        {
            public string Token { get; set; }
        }
    }
}
