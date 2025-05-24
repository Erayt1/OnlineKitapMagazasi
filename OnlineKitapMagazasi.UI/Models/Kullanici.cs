using System.ComponentModel.DataAnnotations;

namespace OnlineKitapMagazasi.UI.Models
{
    public class Kullanici
    {
        [Required]
        public string? KullaniciAdi { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Sifre { get; set; }
    }
}
