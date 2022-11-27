using Microsoft.AspNetCore.Mvc;
using Store.Data.Repositories.IRepositories;
using Store.Models;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository productRepo, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo= productRepo;
            _webHostEnvironment=webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _productRepo.FindAll(includeProperties: "Category");

            return View(products);
        }
    }
}