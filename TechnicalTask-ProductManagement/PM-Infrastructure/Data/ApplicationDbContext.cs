using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PM_Domain.Entities;  // Your custom entities (example: Product, Category, etc.)
using Microsoft.AspNetCore.Identity;

namespace PM_Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // Use your custom ApplicationUser
    {
        // Constructor to inject DbContextOptions into the base class
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

            // Additional custom entity configurations here if needed
        }
    }
}
