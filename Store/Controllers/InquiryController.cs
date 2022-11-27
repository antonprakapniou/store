using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Data;
using Store.Data.Repositories.IRepositories;
using Store.Models;
using Store.Models.ViewModels;
using Store.Utilities.Extensions;
using System.Data;

namespace Store.Controllers
{
    [Authorize(Roles = WebConstants.AdminRole)]
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _inquiryHeaderRepo;
        private readonly IInquiryDetailsRepository _inquiryDetailsRepo;

        [BindProperty]
        public InquiryViewModel? InquiryViewModel { get; set; }

        public InquiryController(IInquiryHeaderRepository inquiryHeaderRepository, IInquiryDetailsRepository inquiryDetailsRepository)
        {
            _inquiryDetailsRepo= inquiryDetailsRepository;
            _inquiryHeaderRepo= inquiryHeaderRepository;
        }

        public IActionResult Index() => View();

        public IActionResult Details(int id)
        {
            InquiryViewModel=new()
            {
                InquiryHeader=_inquiryHeaderRepo.FirstOrDefault(_ => _.Id==id),
                InquiryDetails=_inquiryDetailsRepo.FindAll(_ => _.InquiryHeaderId==id, includeProperties: "Product")
            };

            return View(InquiryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
        public IActionResult DetailsPost()
        {
            List<ShoppingCart> shoppingCarts = new();
            InquiryViewModel!.InquiryDetails=_inquiryDetailsRepo.FindAll(_ => _.InquiryHeaderId==InquiryViewModel.InquiryHeader!.Id);

            foreach (var detail in InquiryViewModel.InquiryDetails)
            {
                ShoppingCart shoppingCart = new()
                {
                    ProductId=detail.ProductId
                };

                shoppingCarts.Add(shoppingCart);
            }

            HttpContext.Session.Clear();
            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCarts);
            HttpContext.Session.Set(WebConstants.SessionInquiry, InquiryViewModel.InquiryHeader!.Id);
            TempData[WebConstants.Success]="Inquiry convert to cart successfully";
            return RedirectToAction(nameof(Index), "Cart");
        }
    }
}