using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kutuphane.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        // Ortak controller özellikleri buraya eklenebilir
    }
} 