using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineKitapMagazasi.API.Models
{
    public class Kitap
    {
        public int Id { get; set; }

        [Required]
        public string? Baslik { get; set; }

        [Required]
        public string? Yazar { get; set; }

        public decimal Fiyat { get; set; }

        public string? ResimUrl { get; set; }

        public ICollection<Yorum>? Yorumlar { get; set; }

    }
}
