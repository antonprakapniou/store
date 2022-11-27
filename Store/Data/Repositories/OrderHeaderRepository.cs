using Store.Data.Repositories.IRepositories;
using Store.Models;

namespace Store.Data.Repositories
{
    public class OrderHeaderRepository:Repository<OrderHeader>,IOrderHeaderRepository
    {
        private readonly AppDbContext _db;

        public OrderHeaderRepository(AppDbContext db) : base(db)
        {
            _db= db;
        }

        public void Update(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Update(orderHeader);
        }
    }
}