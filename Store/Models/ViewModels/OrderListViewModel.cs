using Microsoft.AspNetCore.Mvc.Rendering;

namespace Store.Models.ViewModels
{
    public class OrderListViewModel
    {
        public IEnumerable<OrderHeader>? OrderHeaderList { get; set; }
        public IEnumerable<SelectListItem>? StatusList { get; set; }
        public string? Status { get; set; }
    }
}