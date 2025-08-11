using Microsoft.AspNetCore.Mvc;

namespace StokTakip.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
