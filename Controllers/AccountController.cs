using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Kutuphane.Models;
using Kutuphane.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kutuphane.Controllers
{
    public class AccountController : Controller
    {
        private readonly KutuphaneDbContext _context;

        public AccountController(KutuphaneDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Kullanıcı adı ve şifre gereklidir.";
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password && u.IsActive);
            
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı adı veya şifre hatalı!";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();        

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                TempData["Error"] = "Tüm alanları doldurunuz.";
                return View();
            }

            if (password != confirmPassword)
            {
                TempData["Error"] = "Şifreler eşleşmiyor.";
                return View();
            }

            if (_context.Users.Any(u => u.Username == username))
            {
                TempData["Error"] = "Bu kullanıcı adı zaten kullanılıyor.";
                return View();
            }

            var user = new User
            {
                Username = username,
                Password = password, // Gerçek uygulamada şifre hashlenmelidir!
                Email = email,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Hesabınız başarıyla oluşturuldu. Şimdi giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
} 