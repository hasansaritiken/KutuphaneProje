namespace Kutuphane.Models;

public class Kitap
{
    public int Id { get; set; }
    public string? KitapAdi { get; set; }
    public string? YazarAdi { get; set; }
    public int BasimYili { get; set; }
    public string? YayinEvi { get; set; }

    public Kategori? Kategori { get; set; }
 }
