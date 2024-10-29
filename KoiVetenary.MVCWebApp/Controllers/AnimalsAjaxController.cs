using Microsoft.AspNetCore.Mvc;

namespace KoiVetenary.MVCWebApp.Controllers
{
    public class AnimalsAjaxController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
