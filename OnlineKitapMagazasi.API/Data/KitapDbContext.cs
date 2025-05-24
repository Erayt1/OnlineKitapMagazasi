using Microsoft.EntityFrameworkCore;
using OnlineKitapMagazasi.API.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OnlineKitapMagazasi.API.Data
{
    public class KitapDbContext : DbContext
    {
        public KitapDbContext(DbContextOptions<KitapDbContext> options) : base(options)
        {
        }

        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kitap>()
                .HasMany(k => k.Yorumlar)
                .WithOne(y => y.Kitap)
                .HasForeignKey(y => y.KitapId);
        }
    }
}
