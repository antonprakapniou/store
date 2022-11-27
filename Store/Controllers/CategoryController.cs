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

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(category);
                _categoryRepo.Save();
                TempData[WebConstants.Success]="Category created successfully";
                return RedirectToAction(nameof(Index));
            }

            else
            {
                TempData[WebConstants.Error]="Error while creating category";
                return View(category);
            }
        }
    }
}