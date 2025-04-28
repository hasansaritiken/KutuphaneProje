using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kutuphane.Data;
using Kutuphane.Models;

namespace Kutuphane.Controllers
{
    public class KitapOduncController : Controller
    {
        private readonly KutuphaneDbContext _context;

        public KitapOduncController(KutuphaneDbContext context)
        {
            _context = context;
        }

        // GET: KitapOdunc
        public async Task<IActionResult> Index()
        {
            var oduncIslemleri = await _context.KitapOdunc
                .Include(k => k.Kitap)
                .Include(o => o.Ogrenci)
                .ToListAsync();
            return View(oduncIslemleri);
        }

        // GET: KitapOdunc/Create
        public IActionResult Create(int? kitapId)
        {
            ViewBag.Kitaplar = _context.Kitaplar.ToList();
            ViewBag.Ogrenciler = _context.Ogrenciler.ToList();
            
            var model = new KitapOdunc();
            if (kitapId.HasValue)
            {
                model.KitapId = kitapId.Value;
            }
            
            return View(model);
        }

        // POST: KitapOdunc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KitapId,OgrenciId")] KitapOdunc kitapOdunc)
        {
            if (ModelState.IsValid)
            {
                kitapOdunc.OduncAlmaTarihi = DateTime.Now;
                kitapOdunc.IadeEdildi = false;
                _context.Add(kitapOdunc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Kitaplar = _context.Kitaplar.ToList();
            ViewBag.Ogrenciler = _context.Ogrenciler.ToList();
            return View(kitapOdunc);
        }

        // GET: KitapOdunc/Iade/5
        public async Task<IActionResult> Iade(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kitapOdunc = await _context.KitapOdunc
                .Include(k => k.Kitap)
                .Include(o => o.Ogrenci)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (kitapOdunc == null)
            {
                return NotFound();
            }

            return View(kitapOdunc);
        }

        // POST: KitapOdunc/Iade/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Iade(int id)
        {
            var kitapOdunc = await _context.KitapOdunc.FindAsync(id);
            if (kitapOdunc == null)
            {
                return NotFound();
            }

            kitapOdunc.IadeEdildi = true;
            kitapOdunc.IadeTarihi = DateTime.Now;
            _context.Update(kitapOdunc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
} 