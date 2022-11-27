using Microsoft.AspNetCore.Mvc;
using Store.Data;
using Store.Utilities.Extensions;
using Store.Data.Repositories.IRepositories;
using Store.Models;
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

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartList = new();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!=null
                &&HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.Count()>0)
            {
                shoppingCartList=HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart)!;
            }

            DetailsViewModel detailsViewModel = new()
            {
                Product=_productRepo.FirstOrDefault(_ => _.Id==id, includeProperties: "Category"),
                ExistsInCart=false
            };

            foreach (var shoppingCart in shoppingCartList)
            {
                if (shoppingCart.ProductId==id) detailsViewModel.ExistsInCart=true;
            }

            return View(detailsViewModel);
        }

        [HttpPost]
        [ActionName("Details")]
        public IActionResult DetailsPost(int id, DetailsViewModel detailsViewModel)
        {
            List<ShoppingCart> shoppingCartList = new();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!=null
                &&HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.Count()>0)
            {
                shoppingCartList=HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart)!;
            }

            shoppingCartList.Add(new ShoppingCart
            {
                ProductId = id,
                Count=detailsViewModel.Product.Temp
            });

            TempData[WebConstants.Success]="Product added to cart";
            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartList = new();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!=null
                &&HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.Count()>0)
            {
                shoppingCartList=HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart)!;
            }

            var shoppingCartToRemove = shoppingCartList.SingleOrDefault(_ => _.ProductId==id);
            if (shoppingCartToRemove!=null) shoppingCartList.Remove(shoppingCartToRemove);

            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
    }
}