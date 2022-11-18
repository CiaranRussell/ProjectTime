using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Models;
using ProjectTime.Utility;

namespace ProjectTime.Data
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        // Seed database with Department before creating user to prevent FK-constraint when creating user
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.EnsureCreated();

            if (!context.departments.Any())
            {
                context.departments.AddRange(new List<Department>()
                    {
                        new Department()
                        {
                            Name = "Project Management Office",
                            Rate = 100,
                            CreateDateTime = DateTime.Now,
                        },
                    });
                context.SaveChanges();
            }
        }

        public void Initialize()
        {
            // Add migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }


            // Create roles if they are not created

            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_SuperUser)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User)).GetAwaiter().GetResult();

                // if roles not created, then create Admin User

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "Admin@projecttime.ie",
                    Email = "Admin@projecttime.ie",
                    FullName = "Admin User",
                    EmailConfirmed = true,
                    DepartmentId = 1,


                }, "Coding@1234!").GetAwaiter().GetResult();

                ApplicationUser user = _db.applicationUsers.FirstOrDefault(u => u.Email == "Admin@projecttime.ie");

                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
            return;

        }

    }
}
