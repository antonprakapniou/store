using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Data.Repositories.IRepositories;
using Store.Models;

namespace Store.Data.Repositories
{
    public class ProductRepository:Repository<Product>,IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db) : base(db)
        {
            _db=db;
        }

        public IEnumerable<SelectListItem> GetSelectListItems(string item)
        {
            return _db.Categories.Select(_ => new SelectListItem
            {
                Text=_.Name,
                Value=_.Id.ToString()
            });
        }

        public void Update(Product product)
        {
            _db.Products.Update(product);
        }
    }
}