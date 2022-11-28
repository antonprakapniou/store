using System.Collections.ObjectModel;

namespace Store.Data
{
    public static class WebConstants
    {
        public const string ImagePath = @"\images\products\";

        public const string SessionCart = "ShoppingCartSession";
        public const string SessionInquiry = "InquirytSession";

        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";

        public const string CategoryName = "Category";

        public const string Success = "Success";
        public const string Error = "Error";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static readonly IEnumerable<string> statusList = new ReadOnlyCollection<string>(
            new List<string>
            {
                StatusPending,
                StatusApproved,
                StatusInProcess,
                StatusShipped,
                StatusCancelled,
                StatusRefunded
            });
    }
}