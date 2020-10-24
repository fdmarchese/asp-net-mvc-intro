using Microsoft.AspNetCore.Mvc;

namespace asp_net_mvc_intro.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
