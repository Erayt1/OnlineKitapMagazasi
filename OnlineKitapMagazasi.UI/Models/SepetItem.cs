namespace OnlineKitapMagazasi.UI.Models
{
    public class SepetItem
    {
        public int KitapId { get; set; }
        public string? Baslik { get; set; }
        public string? Yazar { get; set; }
        public decimal Fiyat { get; set; }
        public string? ResimUrl { get; set; }
        public int Adet { get; set; } = 1;
    }
}
