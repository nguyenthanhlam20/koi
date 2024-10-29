using Microsoft.AspNetCore.Mvc;

namespace KoiVetenary.MVCWebApp.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
