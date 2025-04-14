namespace Kutuphane.Models;

public class Sinif
{
    public int Id { get; set; }
    public string? SinifAdi { get; set; }

    public ICollection<Ogrenci>? Ogrenciler { get; set; }
    
}