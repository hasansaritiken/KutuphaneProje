using Microsoft.AspNetCore.Mvc;
using Kutuphane.Data; 
using Kutuphane.Models; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Kutuphane.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly KutuphaneDbContext _context;

        public OgrenciController(KutuphaneDbContext context)
        {
            _context = context;
        }

        // GET: Ogrenci
        public async Task<IActionResult> Index()
        {
            var ogrenciler = await _context.Ogrenciler
                .Include(o => o.Sinif)
                .Where(o => !o.IsDeleted)
                .ToListAsync();
            return View(ogrenciler);
        }

        public async Task<IActionResult> Ekle()
        {
            ViewBag.Siniflar = new SelectList(await _context.Siniflar.ToListAsync(), "Id", "SinifAdi");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Ogrenci ogrenci)
        {
             if (ModelState.IsValid)
            {
                _context.Ogrenciler.Add(ogrenci);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
           ViewBag.Siniflar = new SelectList(await _context.Siniflar.ToListAsync(), "Id", "SinifAdi");
            return View(ogrenci);
        }


        [HttpGet]
        public async Task<IActionResult> Guncelle(int id)
        {
            var ogrenci = await _context.Ogrenciler.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
            if (ogrenci == null) return NotFound();

            ViewBag.Siniflar = new SelectList(await _context.Siniflar.ToListAsync(), "Id", "SinifAdi");
            return View(ogrenci);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guncelle(Ogrenci ogrenci)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Siniflar = new SelectList(await _context.Siniflar.ToListAsync(), "Id", "SinifAdi");
                return View(ogrenci);
            }

            var existingOgrenci = await _context.Ogrenciler.FindAsync(ogrenci.Id);
            if (existingOgrenci == null || existingOgrenci.IsDeleted)
                return NotFound();

            existingOgrenci.OgrenciAdi = ogrenci.OgrenciAdi;
            existingOgrenci.OgrenciSoyadi = ogrenci.OgrenciSoyadi;
            existingOgrenci.OkulNumarasi = ogrenci.OkulNumarasi;
            existingOgrenci.SinifId = ogrenci.SinifId;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Sil(int id)
        {
            var ogrenci = await _context.Ogrenciler.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
            if (ogrenci == null) return NotFound();

            return View(ogrenci); 
        }

        [HttpPost, ActionName("Sil")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SilOnayla(int id)
        {
            var ogrenci = await _context.Ogrenciler.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
            if (ogrenci == null) return NotFound();

            ogrenci.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index"); 
        }
    }
}