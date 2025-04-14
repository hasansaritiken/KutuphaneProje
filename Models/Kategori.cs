namespace Kutuphane.Models;

public class Kategori
{
    public int Id { get; set; }
    public string? KategoriAdi { get; set; }
    public string? Aciklama { get; set; }

    public ICollection<Kitap>? Kitaplar { get; set; }
}
