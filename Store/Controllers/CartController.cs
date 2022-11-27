using Microsoft.AspNetCore.Mvc;
using Store.Data.Repositories.IRepositories;
using Store.Data;
using Store.Models.ViewModels;
using Store.Models;
using Store.Utilities.Braintree;
using Store.Utilities.Extensions;

namespace Store.Controllers
{
    public class CartController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUserRepository _userRepo;
        private readonly IInquiryHeaderRepository _inquiryHeaderRepo;
        private readonly IInquiryDetailsRepository _inquiryDetailsRepo;
        private readonly IOrderHeaderRepository _orderHeaderRepo;
        private readonly IOrderDetailsRepository _orderDetailsRepo;
        private readonly IBraintreeGate _braintree;

        [BindProperty]
        public ProductUserViewModel? ProductUserViewModel { get; set; }

        public CartController(
            ICategoryRepository categoryRepo,
            IProductRepository productRepo,
            IUserRepository userRepo,
            IInquiryHeaderRepository inquiryHeaderRepo,
            IInquiryDetailsRepository inquiryDetailsRepo,
            IOrderHeaderRepository orderHeaderRepository,
            IOrderDetailsRepository orderDetailsRepository,
            IBraintreeGate braintree)
        {
            _categoryRepo=categoryRepo;
            _productRepo=productRepo;
            _userRepo=userRepo;
            _inquiryHeaderRepo=inquiryHeaderRepo;
            _inquiryDetailsRepo=inquiryDetailsRepo;
            _orderHeaderRepo=orderHeaderRepository;
            _orderDetailsRepo=orderDetailsRepository;
            _braintree=braintree;
        }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!=null
                &&HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.Count()>0)
            {
                shoppingCartList=HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.ToList();
            }

            List<int> productInCart = shoppingCartList.Select(_ => _.ProductId).ToList();
            IEnumerable<Product> productListTemp = _productRepo.FindAll(_ => productInCart.Contains(_.Id));
            List<Product> productList = new();

            foreach (var item in shoppingCartList)
            {
                Product productTemp = productListTemp.FirstOrDefault(_ => _.Id==item.ProductId)!;
                productTemp.Temp=item.Count;
                productList.Add(productTemp);
            }

            return View(productList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost(IEnumerable<Product> products)
        {
            List<ShoppingCart> shoppingCarts = new();

            foreach (var product in products)
            {
                shoppingCarts.Add(new ShoppingCart { ProductId=product.Id, Count=product.Temp });
            }

            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCarts);
            return RedirectToAction(nameof(Index));
        }

    }
}