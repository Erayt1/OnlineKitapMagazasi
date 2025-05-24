using Microsoft.AspNetCore.Mvc;
using OnlineKitapMagazasi.UI.Models;
using System.Text.Json;

namespace OnlineKitapMagazasi.UI.Controllers
{
    public class SepetController : Controller
    {
        private const string SepetKey = "sepet";
        
        public IActionResult Index()    // Sepeti getirip listeleyen sayfa
        {
            var sepet = GetSepet();
            return View(sepet);
        }

        [HttpPost]
        public IActionResult SepeteEkle(SepetItem item)
        {
            var sepet = GetSepet();
            var mevcut = sepet.FirstOrDefault(x => x.KitapId == item.KitapId);

            if (mevcut != null)
                mevcut.Adet += 1;
            else
                sepet.Add(item);

            SaveSepet(sepet);

            return RedirectToAction("Index");
        }

        // Sepetten tamamen sil
        [HttpPost]
        public IActionResult Sil(int id)
        {
            var sepet = GetSepet();
            var item = sepet.FirstOrDefault(x => x.KitapId == id);
            if (item != null)
                sepet.Remove(item);

            SaveSepet(sepet);
            return RedirectToAction("Index");
        }

        private List<SepetItem> GetSepet()
        {
            var json = HttpContext.Session.GetString(SepetKey);
            return string.IsNullOrEmpty(json)
                ? new List<SepetItem>()
                : JsonSerializer.Deserialize<List<SepetItem>>(json);
        }

        private void SaveSepet(List<SepetItem> sepet)
        {
            var json = JsonSerializer.Serialize(sepet);
            HttpContext.Session.SetString(SepetKey, json);
        }

        [HttpPost]
        public IActionResult Arttir(int id)
        {
            var sepet = GetSepet();
            var item = sepet.FirstOrDefault(x => x.KitapId == id);
            if (item != null)
                item.Adet++;

            SaveSepet(sepet);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Azalt(int id)
        {
            var sepet = GetSepet();
            var item = sepet.FirstOrDefault(x => x.KitapId == id);
            if (item != null)
            {
                item.Adet--;
                if (item.Adet <= 0)
                    sepet.Remove(item);
            }

            SaveSepet(sepet);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SatinAl()
        {
            HttpContext.Session.Remove("sepet"); 
            return RedirectToAction("SiparisOnay");
        }

        [HttpGet]
        public IActionResult SiparisOnay()
        {
            return View();
        }


    }
}
