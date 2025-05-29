using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kutuphane.Models;
using Kutuphane.Data;
using Microsoft.AspNetCore.Authorization;

namespace Kutuphane.Controllers
{
    [Authorize]
    public class RaporController : Controller
    {
        private readonly KutuphaneDbContext _context;

        public RaporController(KutuphaneDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Siniflar = _context.Siniflar.ToList();
            ViewBag.Kitaplar = _context.Kitaplar.OrderBy(k => k.KitapAdi).ToList();
            ViewBag.Ogrenciler = _context.Ogrenciler
                .Where(o => !o.IsDeleted)
                .OrderBy(o => o.OgrenciAdi)
                .ThenBy(o => o.OgrenciSoyadi)
                .ToList();
            return View();
        }

        [HttpGet]
        public IActionResult TumOduncKitaplar(int? sinifId, int? kitapId, int? ogrenciId, DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            try
            {
                var query = _context.KitapOduncler
                    .Include(k => k.Kitap)
                    .Include(k => k.Ogrenci)
                    .ThenInclude(o => o.Sinif)
                    .Where(k => !k.IsDeleted);

                if (sinifId.HasValue)
                {
                    query = query.Where(k => k.Ogrenci.SinifId == sinifId);
                }

                if (kitapId.HasValue)
                {
                    query = query.Where(k => k.KitapId == kitapId);
                }

                if (ogrenciId.HasValue)
                {
                    query = query.Where(k => k.OgrenciId == ogrenciId);
                }

                if (baslangicTarihi.HasValue)
                {
                    query = query.Where(k => k.AlisTarihi >= baslangicTarihi.Value);
                }

                if (bitisTarihi.HasValue)
                {
                    query = query.Where(k => k.AlisTarihi <= bitisTarihi.Value);
                }

                var oduncler = query
                    .OrderByDescending(k => k.AlisTarihi)
                    .ToList();

                var sonuclar = oduncler.Select(k => new
                {
                    k.Id,
                    k.AlisTarihi,
                    k.SonTeslimTarihi,
                    k.IadeEdildi,
                    k.IadeTarihi,
                    Kitap = k.Kitap != null ? new
                    {
                        k.Kitap.Id,
                        k.Kitap.KitapAdi
                    } : null,
                    Ogrenci = k.Ogrenci != null ? new
                    {
                        k.Ogrenci.Id,
                        k.Ogrenci.OgrenciAdi,
                        k.Ogrenci.OgrenciSoyadi,
                        Sinif = k.Ogrenci.Sinif != null ? new
                        {
                            k.Ogrenci.Sinif.Id,
                            k.Ogrenci.Sinif.SinifAdi
                        } : null
                    } : null
                }).ToList();

                return Json(new { success = true, data = sonuclar });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult EnCokOkuyanOgrenciler(int? sinifId, DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            var query = _context.KitapOduncler
                .Include(k => k.Ogrenci)
                .ThenInclude(o => o.Sinif)
                .Where(k => k.IadeEdildi && !k.IsDeleted);

            if (sinifId.HasValue)
            {
                query = query.Where(k => k.Ogrenci.SinifId == sinifId);
            }

            if (baslangicTarihi.HasValue)
            {
                query = query.Where(k => k.AlisTarihi >= baslangicTarihi.Value);
            }

            if (bitisTarihi.HasValue)
            {
                query = query.Where(k => k.AlisTarihi <= bitisTarihi.Value);
            }

            var enCokOkuyanlar = query
                .GroupBy(k => new { k.OgrenciId, k.Ogrenci.OgrenciAdi, k.Ogrenci.OgrenciSoyadi, k.Ogrenci.Sinif.SinifAdi })
                .Select(g => new
                {
                    OgrenciAdi = g.Key.OgrenciAdi,
                    OgrenciSoyadi = g.Key.OgrenciSoyadi,
                    SinifAdi = g.Key.SinifAdi,
                    OkunanKitapSayisi = g.Count()
                })
                .OrderByDescending(x => x.OkunanKitapSayisi)
                .ToList();

            return Json(enCokOkuyanlar);
        }

        [HttpGet]
        public IActionResult EnCokOkunanKitaplar(int? sinifId, DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            var query = _context.KitapOduncler
                .Include(k => k.Kitap)
                .Include(k => k.Ogrenci)
                .ThenInclude(o => o.Sinif)
                .Where(k => k.IadeEdildi && !k.IsDeleted);

            if (sinifId.HasValue)
            {
                query = query.Where(k => k.Ogrenci.SinifId == sinifId);
            }

            if (baslangicTarihi.HasValue)
            {
                query = query.Where(k => k.AlisTarihi >= baslangicTarihi.Value);
            }

            if (bitisTarihi.HasValue)
            {
                query = query.Where(k => k.AlisTarihi <= bitisTarihi.Value);
            }

            var enCokOkunanlar = query
                .GroupBy(k => new { k.KitapId, k.Kitap.KitapAdi })
                .Select(g => new
                {
                    KitapAdi = g.Key.KitapAdi,
                    OkunmaSayisi = g.Count()
                })
                .OrderByDescending(x => x.OkunmaSayisi)
                .ToList();

            return Json(enCokOkunanlar);
        }

        [HttpGet]
        public IActionResult GecikenKitaplar(int? sinifId)
        {
            try
            {
                var query = _context.KitapOduncler
                    .Include(k => k.Kitap)
                    .Include(k => k.Ogrenci)
                    .ThenInclude(o => o.Sinif)
                    .Where(k => !k.IadeEdildi && !k.IsDeleted && k.SonTeslimTarihi < DateTime.Now);

                if (sinifId.HasValue)
                {
                    query = query.Where(k => k.Ogrenci.SinifId == sinifId);
                }

                var gecikenler = query
                    .Select(k => new
                    {
                        KitapAdi = k.Kitap.KitapAdi,
                        OgrenciAdi = k.Ogrenci.OgrenciAdi,
                        OgrenciSoyadi = k.Ogrenci.OgrenciSoyadi,
                        SinifAdi = k.Ogrenci.Sinif.SinifAdi,
                        AlisTarihi = k.AlisTarihi,
                        SonTeslimTarihi = k.SonTeslimTarihi,
                        GecikmeGunSayisi = (DateTime.Now - k.SonTeslimTarihi).Days
                    })
                    .OrderByDescending(x => x.GecikmeGunSayisi)
                    .ToList();

                return Json(new { success = true, data = gecikenler });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult OduncVerilmisKitaplar(int? sinifId)
        {
            try
            {
                var query = _context.KitapOduncler
                    .Include(k => k.Kitap)
                    .Include(k => k.Ogrenci)
                    .ThenInclude(o => o.Sinif)
                    .Where(k => !k.IadeEdildi && !k.IsDeleted);

                if (sinifId.HasValue)
                {
                    query = query.Where(k => k.Ogrenci.SinifId == sinifId);
                }

                var oduncVerilmisler = query
                    .Select(k => new
                    {
                        KitapAdi = k.Kitap.KitapAdi,
                        OgrenciAdi = k.Ogrenci.OgrenciAdi,
                        OgrenciSoyadi = k.Ogrenci.OgrenciSoyadi,
                        SinifAdi = k.Ogrenci.Sinif.SinifAdi,
                        AlisTarihi = k.AlisTarihi,
                        SonTeslimTarihi = k.SonTeslimTarihi
                    })
                    .OrderByDescending(x => x.AlisTarihi)
                    .ToList();

                return Json(new { success = true, data = oduncVerilmisler });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult OgrenciKitapListesi(int ogrenciId)
        {
            var kitaplar = _context.KitapOduncler
                .Include(k => k.Kitap)
                .Where(k => k.OgrenciId == ogrenciId && k.IadeEdildi && !k.IsDeleted)
                .Select(k => new
                {
                    KitapAdi = k.Kitap.KitapAdi,
                    AlisTarihi = k.AlisTarihi,
                    IadeTarihi = k.IadeTarihi
                })
                .OrderByDescending(x => x.AlisTarihi)
                .ToList();

            return Json(kitaplar);
        }

        [HttpGet]
        public IActionResult KitapOgrenciListesi(int kitapId)
        {
            var ogrenciler = _context.KitapOduncler
                .Include(k => k.Ogrenci)
                .ThenInclude(o => o.Sinif)
                .Where(k => k.KitapId == kitapId && k.IadeEdildi && !k.IsDeleted)
                .Select(k => new
                {
                    OgrenciAdi = k.Ogrenci.OgrenciAdi,
                    OgrenciSoyadi = k.Ogrenci.OgrenciSoyadi,
                    SinifAdi = k.Ogrenci.Sinif.SinifAdi,
                    AlisTarihi = k.AlisTarihi,
                    IadeTarihi = k.IadeTarihi
                })
                .OrderByDescending(x => x.AlisTarihi)
                .ToList();

            return Json(ogrenciler);
        }

        [HttpGet]
        public IActionResult SilinenKayitlar(int? sinifId, int? kitapId, int? ogrenciId, DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            var query = _context.KitapOduncler
                .Include(k => k.Kitap)
                .Include(k => k.Ogrenci)
                .ThenInclude(o => o.Sinif)
                .Where(k => k.IsDeleted);

            if (sinifId.HasValue)
            {
                query = query.Where(k => k.Ogrenci.SinifId == sinifId);
            }

            if (kitapId.HasValue)
            {
                query = query.Where(k => k.KitapId == kitapId);
            }

            if (ogrenciId.HasValue)
            {
                query = query.Where(k => k.OgrenciId == ogrenciId);
            }

            if (baslangicTarihi.HasValue)
            {
                query = query.Where(k => k.AlisTarihi >= baslangicTarihi.Value);
            }

            if (bitisTarihi.HasValue)
            {
                query = query.Where(k => k.AlisTarihi <= bitisTarihi.Value);
            }

            var silinenler = query
                .OrderByDescending(k => k.AlisTarihi)
                .ToList();

            return Json(silinenler);
        }
    }
} 