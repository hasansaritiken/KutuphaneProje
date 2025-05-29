namespace Kutuphane.Models;

public class Ogrenci{
    public int Id { get; set; }
    public string? OgrenciAdi { get; set; }
    public string? OgrenciSoyadi { get; set; }
    public string? OkulNumarasi { get; set; }
    public int? SinifId { get; set; }
    public Sinif? Sinif { get; set; }
    public bool IsDeleted { get; set; }
}