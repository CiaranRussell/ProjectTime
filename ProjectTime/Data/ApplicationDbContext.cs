using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectTime.Models;

namespace ProjectTime.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Department> departments { get; set; }

        public DbSet<Project> projects { get; set; }

        public DbSet<ApplicationUser> applicationUsers { get; set; }

        
    }
}