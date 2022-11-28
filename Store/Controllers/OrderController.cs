using Microsoft.AspNetCore.Mvc;
using Store.Data;
using Store.Data.Repositories.IRepositories;
using Store.Models;
using Store.Models.ViewModels;
using Store.Utilities.Braintree;

namespace Store.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _orderHeaderRepo;
        private readonly IOrderDetailsRepository _orderDetailsRepo;
        private readonly IBraintreeGate _braintree;

        [BindProperty]
        public OrderViewModel? OrderViewModel { get; set; }

        public OrderController(
            IOrderHeaderRepository orderHeaderRepo,
            IOrderDetailsRepository orderDetailsRepo,
            IBraintreeGate braintree)
        {
            _orderHeaderRepo= orderHeaderRepo;
            _orderDetailsRepo= orderDetailsRepo;
            _braintree= braintree;
        }
        
        public IActionResult Index(string ? searchEmail = null, string? Status = null)
        {
            OrderListViewModel orderListViewModel = new()
            {
                OrderHeaderList=_orderHeaderRepo.FindAll(),
                StatusList=WebConstants.statusList.ToList().Select(
                    _=>new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Text=_,
                        Value=_
                    })
            };
            
            if (!string.IsNullOrEmpty(searchEmail))
            {
                orderListViewModel.OrderHeaderList = orderListViewModel.OrderHeaderList.Where(_ => _.Email!.ToLower().Contains(searchEmail.ToLower()));
            }

            if (!string.IsNullOrEmpty(Status) && Status!= "--Order Status--")
            {
                orderListViewModel.OrderHeaderList = orderListViewModel.OrderHeaderList.Where(_ => _.OrderStatus!.ToLower().Contains(Status.ToLower()));
            }

            return View(orderListViewModel);
        }

        public IActionResult Details(int id)
        {
            OrderViewModel = new()
            {
                OrderHeader = _orderHeaderRepo.FirstOrDefault(_ => _.Id == id),
                OrderDetails = _orderDetailsRepo.FindAll(_ => _.OrderHeaderId == id, includeProperties: "Product")
            };

            return View(OrderViewModel);
        }

        [HttpPost]
        public IActionResult StartProcessing()
        {
            OrderHeader orderHeader = _orderHeaderRepo.FirstOrDefault(_ => _.Id == OrderViewModel!.OrderHeader!.Id);
            orderHeader.OrderStatus = WebConstants.StatusInProcess;
            _orderHeaderRepo.Save();
            TempData[WebConstants.Success] = "Order is In Process";
            return RedirectToAction(nameof(Index));
        }
    }
}