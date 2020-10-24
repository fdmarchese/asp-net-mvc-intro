using Microsoft.AspNetCore.Mvc;

namespace ejemplo_cta_cte.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
