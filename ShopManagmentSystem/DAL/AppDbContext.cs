using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
       
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeePosition> EmployeePostions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<RefundOrder> RefundOrders { get; set; }
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Punishment> Punishment { get; set; }
        public DbSet<Money> Moneys { get; set; }
        public DbSet<Displacement> Displacement { get; set; }
        public DbSet<DisplacementProduct> DisplacementProducts { get; set; }
        public DbSet<Message> Messages { get; set; }


    }
}
