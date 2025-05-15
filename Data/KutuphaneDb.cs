using Kutuphane.Models;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Data{
    public class KutuphaneDbContext: DbContext{
        public KutuphaneDbContext(DbContextOptions<KutuphaneDbContext> options): base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Sinif> Siniflar { get; set; }
        public DbSet<Ogrenci> Ogrenciler { get; set; }
        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<KitapOdunc> KitapOduncler { get; set; }
    }
}