namespace Store.Models.ViewModels
{
    public class DetailsViewModel
    {
        public Product Product { get; set; }
        public bool ExistsInCart { get; set; }

        public DetailsViewModel()
        {
            Product= new();
        }
    }
}