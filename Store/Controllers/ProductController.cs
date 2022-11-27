using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}