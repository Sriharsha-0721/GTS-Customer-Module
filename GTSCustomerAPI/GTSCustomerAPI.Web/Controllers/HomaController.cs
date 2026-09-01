using Microsoft.AspNetCore.Mvc;

namespace GTSCustomerAPI.Web.Controllers;

public class HomeController : Controller
{
    // GET: / — blank landing page, only header shows
    public IActionResult Index()
    {
        return View();
    }
}