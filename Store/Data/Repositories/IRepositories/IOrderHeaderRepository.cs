using Store.Models;

namespace Store.Data.Repositories.IRepositories
{
    public interface IOrderHeaderRepository:IRepository<OrderHeader>
    {
        public void Update(OrderHeader orderHeader);
    }
}