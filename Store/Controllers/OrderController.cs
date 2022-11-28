using Microsoft.AspNetCore.Mvc;
using Store.Data.Repositories.IRepositories;
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
        
        public IActionResult Index()
        {
            return View();
        }
    }
}