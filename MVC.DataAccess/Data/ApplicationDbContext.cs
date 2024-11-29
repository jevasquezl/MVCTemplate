using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System.Reflection;

namespace MVC.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Store> Store {  get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Product> Product { get; set; }

        public DbSet<StoreProduct> StoreProduct { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<Inventory> Inventory { get; set; }

        public DbSet<InventoryDetail> InventoryDetail { get; set; }

        public DbSet<Kardex> Kardex { get; set; }

        public DbSet<Company> Company { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<OrderDetail> OrderDetail { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
