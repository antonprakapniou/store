using Microsoft.AspNetCore.Identity;
using Store.Data.Repositories.IRepositories;

namespace Store.Data.Repositories
{
    public class UserRepository : Repository<IdentityUser>, IUserRepository
    {
        private readonly AppDbContext _db;
        
        public UserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}