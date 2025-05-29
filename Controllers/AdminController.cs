using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Kutuphane.Models;
using Kutuphane.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kutuphane.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        private readonly KutuphaneDbContext _context;

        public AdminController(KutuphaneDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Sadece admin kullanıcıları erişebilir
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool IsAdmin()
        {
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            return user?.IsAdmin ?? false;
        }
    }
} 