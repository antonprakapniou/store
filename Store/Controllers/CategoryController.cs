using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Data;
using Store.Data.Repositories.IRepositories;
using Store.Models;
using System.Data;

namespace Store.Controllers
{
    [Authorize(Roles = WebConstants.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo=categoryRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _categoryRepo.FindAll();
            return View(categories);
        }
    }
}