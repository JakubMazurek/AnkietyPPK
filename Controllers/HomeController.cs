using Microsoft.AspNetCore.Mvc;

namespace AnkietyPPK.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
