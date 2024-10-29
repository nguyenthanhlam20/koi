using Microsoft.AspNetCore.Mvc;

namespace KoiVetenary.MVCWebApp.Controllers
{
    public class AppointmentDetailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
