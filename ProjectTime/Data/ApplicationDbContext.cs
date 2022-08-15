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

        public DbSet<ProjectUser> projectUsers { get; set; }


        // On model creation method to loop through all tables with FK relationships & restrict cascade deletion of child records
        // when deleting parent value
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
    }
}