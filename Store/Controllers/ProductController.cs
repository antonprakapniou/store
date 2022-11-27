using Microsoft.AspNetCore.Mvc;
using Store.Data;
using Store.Data.Repositories.IRepositories;
using Store.Models;
using Store.Models.ViewModels;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository productRepo, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo= productRepo;
            _webHostEnvironment=webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _productRepo.FindAll(includeProperties: "Category");

            return View(products);
        }

        public IActionResult Upsert(int? id)
        {
            ProductViewModel productViewModel = new()
            {
                Product=new(),
                CategorySelectList=_productRepo.GetSelectListItems(WebConstants.CategoryName)
            };

            bool productNotExists = id==null;

            if (productNotExists) return View(productViewModel);
            else
            {
                productViewModel.Product=_productRepo.Find(id.GetValueOrDefault());
                bool productIsFounded = productViewModel.Product==null;

                if (productIsFounded) return NotFound();
                return View(productViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
            bool productIsValid = ModelState.IsValid;

            if (productIsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productViewModel.Product!.Id==0)
                {
                    string upload = webRootPath+WebConstants.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    string fullFileName = fileName+extension;

                    using (var fileStream = new FileStream(Path.Combine(upload, fullFileName), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productViewModel.Product.ImagePath=fullFileName;
                    _productRepo.Add(productViewModel.Product);
                    TempData[WebConstants.Success]="Product created successfully";
                }

                else
                {
                    var productFromDb = _productRepo.FirstOrDefault(
                        _ => _.Id==productViewModel.Product.Id,
                        isTracking: false);

                    if (files.Count>0)
                    {
                        string upload = webRootPath+WebConstants.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);
                        string fullFileName = fileName+extension;

                        var oldFile = Path.Combine(upload, productFromDb!.ImagePath!);

                        if (System.IO.File.Exists(oldFile)) System.IO.File.Delete(oldFile);

                        using (var fileStream = new FileStream(Path.Combine(upload, fullFileName), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productViewModel.Product.ImagePath=fullFileName;
                    }

                    else productViewModel.Product.ImagePath=productFromDb!.ImagePath!;

                    _productRepo.Update(productViewModel.Product);
                    TempData[WebConstants.Success]="Product updated successfully";
                }

                _productRepo.Save();
                return RedirectToAction(nameof(Index));
            }

            else
            {
                productViewModel.CategorySelectList=_productRepo.GetSelectListItems(WebConstants.CategoryName);
                TempData[WebConstants.Error]="Error while creating/updating product";

                return View(productViewModel);
            }
        }

        public IActionResult Delete(int? id)
        {
            bool productNotExists = id==null||id==0;

            if (productNotExists) return NotFound();
            else
            {
                var productFromDb = _productRepo.FirstOrDefault(_ => _.Id==id, includeProperties: "Category");
                bool productFromDbNotFound = id==null;

                if (productFromDbNotFound) return NotFound();
                else return View(productFromDb);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var productFromDb = _productRepo.Find(id.GetValueOrDefault());
            bool productFromDbNotFound = id==null;

            if (productFromDbNotFound)
            {
                TempData[WebConstants.Error]="Error while deleting product";
                return NotFound();
            }

            else
            {
                string upload = _webHostEnvironment.WebRootPath+WebConstants.ImagePath;
                var oldFile = Path.Combine(upload, productFromDb!.ImagePath!);

                if (System.IO.File.Exists(oldFile)) System.IO.File.Delete(oldFile);
                _productRepo.Remove(productFromDb);
                _productRepo.Save();
                TempData[WebConstants.Success]="Product deleted successfully";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}