using System.ComponentModel.DataAnnotations;

namespace OnlineKitapMagazasi.UI.Models
{
    public class Yorum
    {
        public int Id { get; set; }
        public string? Icerik { get; set; }
        public int KitapId { get; set; }
    }
}
