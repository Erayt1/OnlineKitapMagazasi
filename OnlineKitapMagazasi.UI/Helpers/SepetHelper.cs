using Microsoft.AspNetCore.Http;
using OnlineKitapMagazasi.UI.Models;
using System.Text.Json;

namespace OnlineKitapMagazasi.UI.Helpers
{
    public static class SepetHelper
    {
        public static int GetSepetAdet(HttpContext context)
        {
            var json = context.Session.GetString("sepet");
            if (string.IsNullOrEmpty(json)) return 0;

            var sepet = JsonSerializer.Deserialize<List<SepetItem>>(json);
            return sepet?.Sum(s => s.Adet) ?? 0;
        }
    }
}
