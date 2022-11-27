using Microsoft.AspNetCore.Mvc;
using Store.Data.Repositories.IRepositories;
using Store.Models.ViewModels;
using System.Diagnostics;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUserRepository _userRepo;

        public HomeController(ILogger<HomeController> logger, ICategoryRepository categoryRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _categoryRepo = categoryRepository;
            _productRepo = productRepository;
            _userRepo = userRepository;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new()
            {
                Products=_productRepo.FindAll(includeProperties: "Category"),
                Categories=_categoryRepo.FindAll()
            };

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}