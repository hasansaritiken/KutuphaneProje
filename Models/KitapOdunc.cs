using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Models;

public class KitapOdunc
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int OgrenciId { get; set; }

    [Required]
    public int KitapId { get; set; }

    [Required]
    [Display(Name = "Alış Tarihi")]
    [DataType(DataType.Date)]
    public DateTime AlisTarihi { get; set; }

    [Required]
    [Display(Name = "Son Teslim Tarihi")]
    [DataType(DataType.Date)]
    public DateTime SonTeslimTarihi { get; set; }

    [Display(Name = "İade Edildi")]
    public bool IadeEdildi { get; set; }

    [Display(Name = "İade Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? IadeTarihi { get; set; }

    [ForeignKey("OgrenciId")]
    [DeleteBehavior(DeleteBehavior.Restrict)]
    public virtual Ogrenci? Ogrenci { get; set; }

    [ForeignKey("KitapId")]
    [DeleteBehavior(DeleteBehavior.Restrict)]
    public virtual Kitap? Kitap { get; set; }

    [NotMapped]
    public int GecikmeGunSayisi
    {
        get
        {
            if (IadeEdildi) return 0;
            return DateTime.Now > SonTeslimTarihi ? (DateTime.Now - SonTeslimTarihi).Days : 0;
        }
    }
} 