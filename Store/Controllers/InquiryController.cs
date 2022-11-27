using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers
{
    public class InquiryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}