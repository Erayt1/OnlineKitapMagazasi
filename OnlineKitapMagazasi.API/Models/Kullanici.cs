using System.ComponentModel.DataAnnotations;

namespace OnlineKitapMagazasi.API.Models
{
    public class Kullanici
    {
        public int Id { get; set; }

        [Required]
        public string? KullaniciAdi { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Sifre { get; set; }

        public string? Rol { get; set; } = "kullanici"; // varsayılan değer
    }
}
