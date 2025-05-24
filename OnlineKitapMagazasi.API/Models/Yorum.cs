using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineKitapMagazasi.API.Models
{
    public class Yorum
    {
        public int Id { get; set; }

        [Required]
        public string? Icerik { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now;

        public int KitapId { get; set; }
        public Kitap? Kitap { get; set; }
    }
}
