using Microsoft.AspNetCore.Mvc;
using Store.Data.Repositories.IRepositories;
using Store.Data;
using Store.Models.ViewModels;
using Store.Models;
using Store.Utilities.Braintree;
using Store.Utilities.Extensions;
using Braintree;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            IdentityUser? identityUser = default;
            bool userIsAdmin = User.IsInRole(WebConstants.AdminRole);

            if (userIsAdmin)
            {
                if (HttpContext.Session.Get<int>(WebConstants.SessionInquiry)!=0)
                {
                    InquiryHeader inquiryHeader = _inquiryHeaderRepo.FirstOrDefault(_ => _.Id==HttpContext.Session.Get<int>(WebConstants.SessionInquiry));
                    identityUser=new()
                    {
                        Email=inquiryHeader.Email,
                    };
                }

                else identityUser=new();

                var gateway = _braintree.GetGateway();
                var clientToken = gateway.ClientToken.Generate();
                ViewBag.ClientToken = clientToken;
            }

            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity!;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                identityUser=_userRepo.FirstOrDefault(_ => _.Id==claim!.Value);
            }
            List<ShoppingCart> shoppingCartList = new();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!=null
                &&HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.Count()>0)
            {
                shoppingCartList=HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.ToList();
            }

            List<int> productInCart = shoppingCartList.Select(_ => _.ProductId).ToList();
            IEnumerable<Product> productList = _productRepo.FindAll(_ => productInCart.Contains(_.Id));

            ProductUserViewModel =new()
            {
                User=identityUser,
            };

            foreach (var item in shoppingCartList)
            {
                Product product = _productRepo.FirstOrDefault(_ => _.Id==item.ProductId);
                product.Temp=item.Count;
                ProductUserViewModel.ProductList!.Add(product);
            }

            return View(ProductUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost(IFormCollection collection, ProductUserViewModel productUserViewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claim = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);
            bool userIaAdmin = User.IsInRole(WebConstants.AdminRole);

            if (userIaAdmin)
            {
                OrderHeader orderHeader = new()
                {
                    CreatedByUserId=claim!.Value,
                    FinalOrderTotal=ProductUserViewModel!.ProductList!.Sum(_ => _.Price*_.Temp),
                    Email=productUserViewModel.User!.Email,
                    OrderDate=DateTime.UtcNow,
                    OrderStatus=WebConstants.StatusPending
                };

                _orderHeaderRepo.Add(orderHeader);
                _orderHeaderRepo.Save();

                foreach (var product in productUserViewModel.ProductList!)
                {
                    OrderDetails orderDetails = new()
                    {
                        OrderHeaderId=orderHeader.Id,
                        ProductId=product.Id,
                        Count=product.Temp,
                        Price=product.Price,
                    };

                    _orderDetailsRepo.Add(orderDetails);
                }

                _orderDetailsRepo.Save();

                string? nonceFromTheClient = collection["payment_method_nonce"];

                var request = new TransactionRequest
                {
                    Amount = Convert.ToDecimal(orderHeader.FinalOrderTotal),
                    PaymentMethodNonce = nonceFromTheClient,
                    OrderId=orderHeader.Id.ToString(),
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    }
                };

                var gateway = _braintree.GetGateway();
                Result<Transaction> result = gateway.Transaction.Sale(request);

                if (result.Target.ProcessorResponseText == "Approved")
                {
                    orderHeader.TransactionId = result.Target.Id;
                    orderHeader.OrderStatus = WebConstants.StatusApproved;
                }
                else
                {
                    orderHeader.OrderStatus = WebConstants.StatusCancelled;
                }

                _orderHeaderRepo.Save();

                return RedirectToAction(nameof(Confirm), new { id = orderHeader.Id });
            }
            else
            {
                InquiryHeader inquiryHeader = new InquiryHeader()
                {
                    UserId = claim!.Value!,
                    Email = ProductUserViewModel!.User!.Email,
                    InquiryDate=DateTime.UtcNow
                };

                _inquiryHeaderRepo.Add(inquiryHeader);
                _inquiryHeaderRepo.Save();

                foreach (var product in ProductUserViewModel!.ProductList!)
                {
                    InquiryDetails inquiryDetails = new InquiryDetails()
                    {
                        InquiryHeaderId=inquiryHeader.Id,
                        ProductId=product.Id,
                    };

                    _inquiryDetailsRepo.Add(inquiryDetails);
                }

                _categoryRepo.Save();
                TempData[WebConstants.Success]="Inquiry created successfully";
            }

            return RedirectToAction(nameof(Confirm));
        }

        public IActionResult Confirm(int id = 0)
        {
            OrderHeader orderHeader = _orderHeaderRepo.FirstOrDefault(_ => _.Id==id);
            HttpContext.Session.Clear();
            return View(orderHeader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Update")]
        public IActionResult UpdatePost(IEnumerable<Product> products)
        {
            List<ShoppingCart> shoppingCarts = new();

            foreach (var product in products)
            {
                shoppingCarts.Add(new ShoppingCart { ProductId=product.Id, Count=product.Temp });
            }

            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCarts);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!=null
                &&HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.Count()>0)
            {
                shoppingCartList=HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.ToList();
            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(_ => _.ProductId==id)!);
            TempData[WebConstants.Success]="Product deleted from cart successfully";
            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }
    }
}