using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Store.Data.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            AppDbContext db,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db= db;
            _userManager= userManager;
            _roleManager= roleManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }

            catch (Exception ex) { }


            if (!_roleManager.RoleExistsAsync(WebConstants.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebConstants.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebConstants.CustomerRole)).GetAwaiter().GetResult();
            }
            else
            {
                return;
            }

            IdentityUser identityUser = new()
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,                
            };

            string? password = "qwerty";

            _userManager.CreateAsync(identityUser,password).GetAwaiter().GetResult();

            IdentityUser? user = _db.Users.FirstOrDefault(_ => _.Email == "admin@gmail.com");
            _userManager.AddToRoleAsync(user!, WebConstants.AdminRole).GetAwaiter().GetResult();
        }
    }
}