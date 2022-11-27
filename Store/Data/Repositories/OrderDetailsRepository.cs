using Store.Data.Repositories.IRepositories;
using Store.Models;

namespace Store.Data.Repositories
{
    public class OrderDetailsRepository:Repository<OrderDetails>,IOrderDetailsRepository
    {
        private readonly AppDbContext _db;

        public OrderDetailsRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetails orderDetails)
        {
            _db.OrderDetails.Update(orderDetails);
        }
    }
}