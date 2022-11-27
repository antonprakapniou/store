using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}