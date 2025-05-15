using System.ComponentModel.DataAnnotations;

namespace Kutuphane.Models;

public enum KitapDurumu
{
    Musait = 0,
    OduncVerildi = 1,
    Kayip = 2
}

public class Kitap
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Kitap Adı")]
    public string KitapAdi { get; set; }

    [Display(Name = "Durum")]
    public KitapDurumu Durum { get; set; } = KitapDurumu.Musait;

    public string? YazarAdi { get; set; }
    public int BasimYili { get; set; }
    public string? YayinEvi { get; set; }

    public Kategori? Kategori { get; set; }
}
