using Kutuphane.Models;
using Kutuphane.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Controllers
{
    public class KitapController : Controller
    {
        private readonly KutuphaneDbContext _context;

        public KitapController(KutuphaneDbContext context)
        {
            _context = context;
        }

        // GET: Kitap
        public async Task<IActionResult> Index()
        {
            var kitaplar = await _context.Kitaplar
                .Include(k => k.Kategori)
                .Where(k => k.Durum != KitapDurumu.Kayip)
                .ToListAsync();
            return View(kitaplar);
        }

        // GET: Kitap/Create
        public IActionResult Create()
        {
            ViewBag.Kategoriler = _context.Kategoriler.ToList();
            return View();
        }

        // POST: Kitap/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Kitap kitap)
        {
            if (kitap.Kategori != null)
            {
                kitap.Kategori = await _context.Kategoriler.FindAsync(kitap.Kategori.Id);
            }

            if (ModelState.IsValid)
            {
                _context.Add(kitap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Kategoriler = _context.Kategoriler.ToList();
            return View(kitap);
        }

        // GET: Kitap/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var kitap = await _context.Kitaplar
                .Include(k => k.Kategori)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kitap == null) return NotFound();

            ViewBag.Kategoriler = _context.Kategoriler.ToList();
            return View(kitap);
        }

        // POST: Kitap/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kitap kitap)
        {
            if (id != kitap.Id) return NotFound();

            if (kitap.Kategori != null)
            {
                kitap.Kategori = await _context.Kategoriler.FindAsync(kitap.Kategori.Id);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kitap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Kitaplar.Any(k => k.Id == kitap.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Kategoriler = _context.Kategoriler.ToList();
            return View(kitap);
        }

        // GET: Kitap/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var kitap = await _context.Kitaplar
                .Include(k => k.Kategori)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kitap == null) return NotFound();

            return View(kitap);
        }

        // POST: Kitap/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kitap = await _context.Kitaplar.FindAsync(id);
            if (kitap != null)
            {
                kitap.Durum = KitapDurumu.Kayip;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
