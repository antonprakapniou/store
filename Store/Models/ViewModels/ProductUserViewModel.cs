using Microsoft.AspNetCore.Identity;

namespace Store.Models.ViewModels
{
    public class ProductUserViewModel
    {
        public IdentityUser? User { get; set; }
        public List<Product>? ProductList { get; set; }

        public ProductUserViewModel()
        {
            ProductList=new();
        }
    }
}