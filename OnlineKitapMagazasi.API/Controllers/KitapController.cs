using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineKitapMagazasi.API.Data;
using OnlineKitapMagazasi.API.Models;

namespace OnlineKitapMagazasi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitapController : ControllerBase
    {
        private readonly KitapDbContext _context;

        public KitapController(KitapDbContext context)
        {
            _context = context;
        }

        // burada tüm kitapları getiriyorum
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kitap>>> TumKitaplariGetir()
        {
            return await _context.Kitaplar.Include(k => k.Yorumlar).ToListAsync();
        }

        // kitapları id sine göre , belirli olanları getiriyorum
        [HttpGet("{id}")]
        public async Task<ActionResult<Kitap>> KitapGetir(int id)
        {
            var kitap = await _context.Kitaplar.Include(k => k.Yorumlar).FirstOrDefaultAsync(k => k.Id == id);

            if (kitap == null)
                return NotFound();

            return kitap;
        }

        //  yeni kitap ekleme -  sadece admin ekleyebilir
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> KitapEkle(Kitap kitap)
        {
            _context.Kitaplar.Add(kitap);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(KitapGetir), new { id = kitap.Id }, kitap);
        }


        // kitapları güncellediğim kodum
        [HttpPut("{id}")]
        public async Task<IActionResult> KitapGuncelle(int id, Kitap kitap)
        {
            if (id != kitap.Id)
                return BadRequest();

            _context.Entry(kitap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Kitaplar.Any(k => k.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Kitap silme işlemi
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> KitapSil(int id)
        {
            var kitap = await _context.Kitaplar.FindAsync(id);

            if (kitap == null)
                return NotFound();

            _context.Kitaplar.Remove(kitap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("arama")]
        public async Task<IActionResult> KitapAra(string q)
        {
            var sonuc = await _context.Kitaplar
                .Where(k => k.Baslik.Contains(q) || k.Yazar.Contains(q))
                .ToListAsync();

            return Ok(sonuc);
        }



    }
}
