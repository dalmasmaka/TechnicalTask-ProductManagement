using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PM_Domain.Entities;  

namespace PM_Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Indexing 'Name' column in the 'Product' table
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();

            // Indexing 'Name' column in the 'Categories' table
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

        }
    }
}
