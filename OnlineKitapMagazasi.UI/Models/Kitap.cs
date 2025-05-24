using System.Collections.Generic;

namespace OnlineKitapMagazasi.UI.Models
{
    public class Kitap
    {
        public int Id { get; set; }
        public string? Baslik { get; set; }
        public string? Yazar { get; set; }
        public decimal Fiyat { get; set; }
        public string? ResimUrl { get; set; }
        public List<Yorum> Yorumlar { get; set; }
    }


}
