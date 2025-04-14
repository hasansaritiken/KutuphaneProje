using Kutuphane.Models;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Data{
    public class KutuphaneDbContext: DbContext{
        public KutuphaneDbContext(DbContextOptions<KutuphaneDbContext> options): base(options)
        {
        }
        public DbSet<Sinif> Siniflar { get; set; }
        public DbSet<Ogrenci> Ogrenciler { get; set; }
    }
}