﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Controllers
{
    public class RefundController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public RefundController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            RefundHomeVM refundHomeVM = new RefundHomeVM();
            refundHomeVM.Refunds =await _context.Refunds
                .Include(r=>r.RefundProducts)
                .Include(r=>r.Customer)
                .Include(r=>r.Employee)
                .Where(r=>r.BranchId == user.BranchId)
                .ToListAsync();
            refundHomeVM.RefundOrders =await _context.RefundOrders
                .Include(ro=>ro.Customer)
                .ToListAsync();
            return View(refundHomeVM);
        }
        public async Task<IActionResult> Order(int? id, int? customerId , int? employeeId)
        {
            if (id == null || id == 0 || customerId == null || customerId == 0 || employeeId == 0 || employeeId == null) return NotFound();
            Product? product = await _context.Products
                .Include(p => p.SaleProducts)
                .Include(p => p.ProductCategory)
                .Include(p => p.Color)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.Id == id);
            Customer? customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            if (product == null || customer == null || employee == null) return NotFound();
            List<RefundOrder> refundOrder = await _context.RefundOrders.ToListAsync();
            if (refundOrder.Any(r => r.ProdId == id)) return Ok("Siyahida movcuddur !");

            RefundOrder order = new();
            order.CreateDate = DateTime.Now;
            order.Brand = product.Brand.BrandName;
            order.Name = product.Name;
            order.ProdId = product.Id;
            order.Price = product.Price;
            order.Series = product.Series;
            order.Color = product.Color.ColorName;
            order.BranchId = product.BranchId;
            order.Category = product.ProductCategory.Name;
            order.CustomerId = customer.Id;
            order.EmployeeName = employee.FullName;

            _context.RefundOrders.Add(order);
            await _context.SaveChangesAsync();

            return Ok($"Geri qaytarma siyahisina elave edildi ! x{_context.RefundOrders.Count()}");
        }
    }
}