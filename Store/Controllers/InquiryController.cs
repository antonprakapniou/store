using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Data;
using Store.Data.Repositories.IRepositories;
using Store.Models.ViewModels;
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
    }
}