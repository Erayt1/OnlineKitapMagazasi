using Microsoft.AspNetCore.Mvc;
using OnlineKitapMagazasi.API.Data;
using OnlineKitapMagazasi.API.Models;
using OnlineKitapMagazasi.API.Services;
using Microsoft.EntityFrameworkCore;

namespace OnlineKitapMagazasi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly KitapDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(KitapDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("kayit")]
        public async Task<IActionResult> KayitOl(Kullanici kullanici)
        {
            var mevcut = await _context.Kullanicilar.AnyAsync(x => x.KullaniciAdi == kullanici.KullaniciAdi);
            if (mevcut)
                return BadRequest("Bu kullanıcı adı zaten var.");

            _context.Kullanicilar.Add(kullanici);
            await _context.SaveChangesAsync();
            return Ok("Kayıt başarılı.");
        }

        [HttpPost("giris")]
        public async Task<IActionResult> GirisYap(KullaniciGirisDto giris)
        {
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.KullaniciAdi == giris.KullaniciAdi && k.Sifre == giris.Sifre);

            if (kullanici == null)
                return Unauthorized("Kullanıcı adı veya şifre yanlış.");

            var token = _tokenService.TokenUret(kullanici);
            return Ok(new { Token = token });
        }




    }
}
