using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Controllers
{
    [Authorize]
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
            refundHomeVM.Refunds = await _context.Refunds
                .Include(r => r.Customer)
                .Include(r => r.Employee)
                .Where(r => r.BranchId == user.BranchId)
                .ToListAsync();
            refundHomeVM.RefundOrders = await _context.RefundOrders
                .Include(ro => ro.Customer)
                .Where(ro => ro.BranchId == user.BranchId)
                .ToListAsync();
            return View(refundHomeVM);
        }
        public async Task<IActionResult> Order(int? id, int? customerId, int? employeeId, int? saleId)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            if (id == null || id == 0 || customerId == null || customerId == 0
                || employeeId == 0 || employeeId == null || saleId == 0 || saleId == null) return NotFound();
            if (!User.Identity.IsAuthenticated) return NotFound();
            Product? product = await _context.Products
                .Include(p => p.SaleProducts)
                .Include(p => p.ProductCategory)
                .Include(p => p.Color)
                .Include(p => p.Brand)
                .Include(p => p.ProductModel)
                .Where(p=>p.BranchId == user.BranchId)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            if (!product.IsSold) return Ok("Bu mehsul artiq qeri qaytarilib !");
            Customer? customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            Sale? sale = await _context.Sales.FirstOrDefaultAsync(s => s.Id == saleId);
            if (product == null || customer == null || employee == null || sale == null) return NotFound();
            List<RefundOrder> refundOrder = await _context.RefundOrders.ToListAsync();
            if (refundOrder.Any(r => r.ProdId == id)) return Ok("Geri qaytarma siyahisinda movcuddur !");

            RefundOrder order = new();
            order.CreateDate = DateTime.Now;
            order.Brand = product.Brand.BrandName;
            order.Name = product.ProductModel.ModelName;
            order.ProdId = product.Id;
            order.Price = product.ProductModel.ModelPrice;
            order.Series = product.Series;
            order.Color = product.Color.ColorName;
            order.BranchId = product.BranchId;
            order.Category = product.ProductCategory.Name;
            order.CustomerId = customer.Id;
            order.EmployeeName = employee.FullName;
            order.BranchId = user.BranchId;
            order.SaleId = sale.Id;

            _context.RefundOrders.Add(order);
            await _context.SaveChangesAsync();

            return Ok($"Geri qaytarma siyahisina elave edildi ! x{_context.RefundOrders.Where(o => o.BranchId == user.BranchId).Count()}");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Refund? refund = await _context.Refunds
                .Include(r => r.Customer)
                .Include(r => r.Product).ThenInclude(p => p.Brand)
                .Include(r => r.Product).ThenInclude(p => p.ProductModel)
                .Include(r => r.Product).ThenInclude(p => p.Color)
                .Include(r => r.Product).ThenInclude(p => p.ProductCategory)
                .Include(r => r.Employee)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (refund == null) return NotFound();


            return View(refund);
        }
    }
}
