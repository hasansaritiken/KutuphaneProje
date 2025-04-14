using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kutuphane.Models;
using Kutuphane.Data;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Controllers;

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
        var Kategoriler  = await _context.Kategoriler.OrderBy(s => s.KategoriAdi).ToListAsync();
        return View(Kategoriler);
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
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var sinif = await _context.Kategoriler.FindAsync(id);
        if (sinif == null)
        {
            return NotFound();
        }
        return View(sinif);
    }

    // POST: Kategori/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Guncelle(Kategori kategori)
    {
        if (ModelState.IsValid)
        {
            _context.Kategoriler.Update(kategori);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(kategori);
    }

    // GET: Kategori/Delete/5
    public async Task<IActionResult> Sil(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var kategori = await _context.Kategoriler.FindAsync(id);
        if (kategori != null)
        {
            _context.Kategoriler.Remove(kategori);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return NotFound();
        }
        
    }

    
}