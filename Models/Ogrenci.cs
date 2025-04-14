namespace Kutuphane.Models;

public class Ogrenci{
    public int Id { get; set; }
    public string? OgrenciAd { get; set; }
    public string? OgrenciSoyad { get; set; }

    public Sinif Sinif { get; set; }
}