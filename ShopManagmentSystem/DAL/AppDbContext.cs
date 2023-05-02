﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public DbSet<EmployeePostion> EmployeePostions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Refund> Refunds { get; set; }




    }
}
