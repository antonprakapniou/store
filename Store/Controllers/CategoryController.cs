using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Data;
using Store.Data.Repositories.IRepositories;
using Store.Models;

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

        public IActionResult Update(int? id)
        {
            if (id>0)
            {
                var category = _categoryRepo.Find(id.GetValueOrDefault());

                if (category==null) return NotFound();
                else return View(category);
            }

            else return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Update")]
        public IActionResult Update(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(category);
                _categoryRepo.Save();
                TempData[WebConstants.Success]="Category updated successfully";
                return RedirectToAction(nameof(Index));
            }

            else
            {
                TempData[WebConstants.Error]="Error while updating category";
                return View(category);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id>0)
            {
                var category = _categoryRepo.Find(id.GetValueOrDefault());

                if (category==null) return NotFound();
                else return View(category);
            }

            else return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var category = _categoryRepo.Find(id.GetValueOrDefault());
            if (category==null)
            {
                TempData[WebConstants.Error]="Error while deleting category";
                return NotFound();
            }

            else
            {
                _categoryRepo.Remove(category);
                _categoryRepo.Save();
                TempData[WebConstants.Success]="Category deleted successfully";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}