using FinTrack_API.Service.IService;
using FinTrack_Common;
using FinTrack_DataAccess;
using FinTrack_DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinTrack_API.Service
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public DbInitializer(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
                if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_User)).GetAwaiter().GetResult();
                }
                else
                {
                    return;
                }

                ApplicationUser user = new()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(user, "Admin@12").GetAwaiter().GetResult();

                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {

            }
        }
    }
}