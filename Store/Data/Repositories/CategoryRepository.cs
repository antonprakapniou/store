using Store.Data.Repositories.IRepositories;
using Store.Models;

namespace Store.Data.Repositories
{
    public class CategoryRepository:Repository<Category>,ICategoryRepository
    {
        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext db) : base(db)
        {
            _db=db;
        }

        public void Update(Category category)
        {
            var categoryFromDb = FirstOrDefault(_ => _.Id==category.Id);

            if (categoryFromDb!=null)
            {
                categoryFromDb.Name=category.Name;
            }
        }
    }
}