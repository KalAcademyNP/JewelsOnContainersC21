using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
