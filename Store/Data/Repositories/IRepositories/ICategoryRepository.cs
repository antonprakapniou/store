using Store.Models;

namespace Store.Data.Repositories.IRepositories
{
    public interface ICategoryRepository:IRepository<Category>
    {
        public void Update(Category category);
    }
}