using Microsoft.AspNetCore.Identity;

namespace Store.Data.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<IdentityUser> { }
}