using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kutuphane.Models;
using Kutuphane.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Kutuphane.Controllers;

[Authorize]
public class KategoriController : Controller
{
    private readonly KutuphaneDbContext _context;

    public KategoriController(KutuphaneDbContext context)
    {
        _context = context;
    }

    // GET: Kategori
    public async Task<IActionResult> Index()
    {
        var kategoriler = await _context.Kategoriler
            .Include(k => k.Kitaplar)
            .OrderBy(k => k.KategoriAdi)
            .ToListAsync();
        return View(kategoriler);
    }

    // GET: Kategori/Create
    public IActionResult Ekle()
    {
        return View();
    }

    // POST: Kategori/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(Kategori kategori)
    {
        if (ModelState.IsValid)
        {
            await _context.Kategoriler.AddAsync(kategori);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(kategori);
    }

    // GET: Kategori/Edit/5
    public async Task<IActionResult> Guncelle(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var kategori = await _context.Kategoriler.FindAsync(id);
        if (kategori == null)
        {
            return NotFound();
        }

        return View(kategori);
    }

    // POST: Kategori/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Guncelle(int id, Kategori kategori)
    {
        if (id != kategori.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(kategori);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Kategoriler.AnyAsync(k => k.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(kategori);
    }

    // GET: Kategori/Delete/5
    public async Task<IActionResult> Sil(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var kategori = await _context.Kategoriler
            .Include(k => k.Kitaplar)
            .FirstOrDefaultAsync(k => k.Id == id);

        if (kategori == null)
        {
            return NotFound();
        }

        if (kategori.Kitaplar?.Any() == true)
        {
            TempData["Error"] = "Bu kategoriye ait kitaplar bulunmaktadır. Önce kitapları başka kategorilere taşıyın veya silin.";
            return RedirectToAction(nameof(Index));
        }

        return View(kategori);
    }

    [HttpPost, ActionName("Sil")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SilOnayla(int id)
    {
        var kategori = await _context.Kategoriler.FindAsync(id);
        if (kategori == null)
        {
            return NotFound();
        }

        _context.Kategoriler.Remove(kategori);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}