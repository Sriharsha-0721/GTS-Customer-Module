using Microsoft.AspNetCore.Mvc;

namespace GTS.MVC.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}