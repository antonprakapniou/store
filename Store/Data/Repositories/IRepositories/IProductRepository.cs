using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Models;

namespace Store.Data.Repositories.IRepositories
{
    public interface IProductRepository:IRepository<Product>
    {
        public void Update(Product product);

        IEnumerable<SelectListItem> GetSelectListItems(string item);
    }
}
