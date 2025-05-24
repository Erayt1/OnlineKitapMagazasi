using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineKitapMagazasi.API.Data;
using OnlineKitapMagazasi.API.Models;

namespace OnlineKitapMagazasi.API.Controllers
{
    [Authorize] // Bu etiket koruma sağlar
    [Route("api/[controller]")]
    [ApiController]
    public class YorumController : ControllerBase
    {
        private readonly KitapDbContext _context;

        public YorumController(KitapDbContext context)
        {
            _context = context;
        }

        // Belirli bir kitabın yorumlarını getiriyorum (tarih sırasına göre)
        [HttpGet("kitap/{kitapId}")]
        public async Task<ActionResult<IEnumerable<Yorum>>> KitabinYorumlari(int kitapId)
        {
            var yorumlar = await _context.Yorumlar
                .Where(y => y.KitapId == kitapId)
                .OrderByDescending(y => y.Tarih)
                .ToListAsync();
            // kitapla alakalı yorum yoksa da boş liste dönecek zaten
            return yorumlar;
        }

        //  Yeni yorum ekliyorum
        [HttpPost]
        public async Task<ActionResult<Yorum>> YorumEkle(Yorum yorum)
        {   
            // kitap var mı diye bakıyorum, yoksa yorum eklenmesin
            var kitapVarMi = await _context.Kitaplar.AnyAsync(k => k.Id == yorum.KitapId);
            if (!kitapVarMi)
            {
                return NotFound("Yorum eklenecek kitap bulunamadı.");
            }

            _context.Yorumlar.Add(yorum);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(YorumGetir), new { id = yorum.Id }, yorum);
        }

        // ID si verilen yorumu döndürür
        [HttpGet("{id}")]
        public async Task<ActionResult<Yorum>> YorumGetir(int id)
        {
            var yorum = await _context.Yorumlar.FindAsync(id);
            if (yorum == null)
                return NotFound();

            return yorum;
        }

        // Yorum silme işlemi (admin değil herkes kendi yorumunu silebilsin diye authorize tek başına)
        [HttpDelete("{id}")]
        public async Task<IActionResult> YorumSil(int id)
        {
            var yorum = await _context.Yorumlar.FindAsync(id);

            if (yorum == null)
                return NotFound();

            _context.Yorumlar.Remove(yorum);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
