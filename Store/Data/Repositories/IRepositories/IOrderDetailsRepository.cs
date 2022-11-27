using Store.Models;

namespace Store.Data.Repositories.IRepositories
{
    public interface IOrderDetailsRepository:IRepository<OrderDetails>
    {
        public void Update(OrderDetails orderDetails);
    }
}