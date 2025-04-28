namespace Kutuphane.Models;

public class KitapOdunc
{
    public int Id { get; set; }
    public int KitapId { get; set; }
    public int OgrenciId { get; set; }
    public DateTime OduncAlmaTarihi { get; set; }
    public DateTime? IadeTarihi { get; set; }
    public bool IadeEdildi { get; set; }

    // Navigation properties
    public Kitap? Kitap { get; set; }
    public Ogrenci? Ogrenci { get; set; }
} 