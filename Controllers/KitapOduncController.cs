using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kutuphane.Models;
using Kutuphane.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Kutuphane.Controllers
{
    [Authorize]
    public class KitapOduncController : Controller
    {
        private readonly KutuphaneDbContext _context;
        private readonly ILogger<KitapOduncController> _logger;

        public KitapOduncController(KutuphaneDbContext context, ILogger<KitapOduncController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var oduncler = _context.KitapOduncler
                .Include(k => k.Kitap)
                .Include(k => k.Ogrenci)
                .Where(k => !k.IadeEdildi)
                .OrderByDescending(k => k.AlisTarihi)
                .ToList();
            return View(oduncler);
        }

        public IActionResult Create()
        {
            ViewBag.Kitaplar = _context.Kitaplar.Where(k => k.Durum == KitapDurumu.Musait).ToList();
            ViewBag.Ogrenciler = _context.Ogrenciler.Where(o => !o.IsDeleted).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(int KitapId, int OgrenciId)
        {
            if (KitapId == 0)
            {
                TempData["Error"] = "Lütfen bir kitap seçin.";
                ViewBag.Kitaplar = _context.Kitaplar.Where(k => k.Durum == KitapDurumu.Musait).ToList();
                ViewBag.Ogrenciler = _context.Ogrenciler.Where(o => !o.IsDeleted).ToList();
                return View();
            }

            if (OgrenciId == 0)
            {
                TempData["Error"] = "Lütfen bir öğrenci seçin.";
                ViewBag.Kitaplar = _context.Kitaplar.Where(k => k.Durum == KitapDurumu.Musait).ToList();
                ViewBag.Ogrenciler = _context.Ogrenciler.Where(o => !o.IsDeleted).ToList();
                return View();
            }

            var kitap = _context.Kitaplar.FirstOrDefault(k => k.Id == KitapId);
            if (kitap == null || kitap.Durum != KitapDurumu.Musait)
            {
                TempData["Error"] = "Seçilen kitap ödünç verilemez durumda.";
                ViewBag.Kitaplar = _context.Kitaplar.Where(k => k.Durum == KitapDurumu.Musait).ToList();
                ViewBag.Ogrenciler = _context.Ogrenciler.Where(o => !o.IsDeleted).ToList();
                return View();
            }

            var ogrenci = _context.Ogrenciler.FirstOrDefault(o => o.Id == OgrenciId && !o.IsDeleted);
            if (ogrenci == null)
            {
                TempData["Error"] = "Seçilen öğrenci bulunamadı veya silinmiş.";
                ViewBag.Kitaplar = _context.Kitaplar.Where(k => k.Durum == KitapDurumu.Musait).ToList();
                ViewBag.Ogrenciler = _context.Ogrenciler.Where(o => !o.IsDeleted).ToList();
                return View();
            }

            var kitapOdunc = new KitapOdunc
            {
                KitapId = KitapId,
                OgrenciId = OgrenciId,
                AlisTarihi = DateTime.Now,
                SonTeslimTarihi = DateTime.Now.AddDays(30),
                IadeEdildi = false
            };

            _context.KitapOduncler.Add(kitapOdunc);
            kitap.Durum = KitapDurumu.OduncVerildi;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult IadeEt(int id)
        {
            var kitapOdunc = _context.KitapOduncler
                .Include(k => k.Kitap)
                .FirstOrDefault(k => k.Id == id && !k.IadeEdildi);

            if (kitapOdunc == null)
            {
                return NotFound();
            }

            kitapOdunc.IadeEdildi = true;
            kitapOdunc.IadeTarihi = DateTime.Now;
            
            if (kitapOdunc.Kitap != null)
            {
                kitapOdunc.Kitap.Durum = KitapDurumu.Musait;
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Rapor()
        {
            var oduncler = _context.KitapOduncler
                .Include(k => k.Kitap)
                .Include(k => k.Ogrenci)
                .OrderByDescending(k => k.AlisTarihi)
                .ToList();
            return View(oduncler);
        }

        public IActionResult GecikenKitaplar()
        {
            var gecikenliler = _context.KitapOduncler
                .Include(k => k.Kitap)
                .Include(k => k.Ogrenci)
                .Where(k => !k.IadeEdildi && k.SonTeslimTarihi < DateTime.Now)
                .OrderBy(k => k.SonTeslimTarihi)
                .ToList();
            return View("Rapor", gecikenliler);
        }
    }
}
