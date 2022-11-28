using Microsoft.AspNetCore.Mvc;
using Store.Data;
using Store.Data.Repositories.IRepositories;
using Store.Models.ViewModels;
using Store.Utilities.Braintree;

namespace Store.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _orderHeaderRepo;
        private readonly IOrderDetailsRepository _orderDetailsRepo;
        private readonly IBraintreeGate _braintree;

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
    }
}