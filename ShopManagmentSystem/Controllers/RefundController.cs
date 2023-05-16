using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels.RefundVMs;

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
        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate)
        {
            RefundHomeVM refundHomeVM = new RefundHomeVM();
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();

            ViewBag.toDate = toDate;
            ViewBag.fromDate = fromDate;

            if (fromDate > toDate)
            {
                ViewBag.Error = "Error Date";
              
                refundHomeVM.Refunds = await _context.Refunds
                    .Include(r => r.Customer)
                    .Include(r => r.Employee)
                    .Where(r => r.BranchId == user.BranchId && r.CreateDate > fromDate)
                    .ToListAsync();
                refundHomeVM.RefundOrders = await _context.RefundOrders
                    .Include(ro => ro.Customer)
                    .Where(ro => ro.BranchId == user.BranchId)
                    .ToListAsync();
                return View(refundHomeVM);
            }
            if(fromDate == null && toDate != null)
            {
                refundHomeVM.Refunds = await _context.Refunds
                    .Include(r => r.Customer)
                    .Include(r => r.Employee)
                    .Where(r => r.BranchId == user.BranchId && r.CreateDate < toDate.Value.AddHours(23))
                    .ToListAsync();
                refundHomeVM.RefundOrders = await _context.RefundOrders
                    .Include(ro => ro.Customer)
                    .Where(ro => ro.BranchId == user.BranchId)
                    .ToListAsync();
                return View(refundHomeVM);
            }
            if(fromDate != null && toDate == null)
            {
                refundHomeVM.Refunds = await _context.Refunds
                .Include(r => r.Customer)
                .Include(r => r.Employee)
                .Where(r => r.BranchId == user.BranchId && r.CreateDate > fromDate)
                .ToListAsync();
                refundHomeVM.RefundOrders = await _context.RefundOrders
                    .Include(ro => ro.Customer)
                    .Where(ro => ro.BranchId == user.BranchId)
                    .ToListAsync();
                return View(refundHomeVM);
            }
            if(fromDate != null && toDate != null)
            {
                refundHomeVM.Refunds = await _context.Refunds
               .Include(r => r.Customer)
               .Include(r => r.Employee)
               .Where(r => r.BranchId == user.BranchId && r.CreateDate > fromDate && r.CreateDate < toDate.Value.AddHours(23))
               .ToListAsync();
                refundHomeVM.RefundOrders = await _context.RefundOrders
                    .Include(ro => ro.Customer)
                    .Where(ro => ro.BranchId == user.BranchId)
                    .ToListAsync();
                return View(refundHomeVM);
            }
            if(fromDate == null && toDate == null)
            {
                ViewBag.fromDate = DateTime.Today;
                ViewBag.toDate = DateTime.Today.AddHours(23);             
            }
            

            refundHomeVM.Refunds = await _context.Refunds
                .Include(r => r.Customer)
                .Include(r => r.Employee)
                .Where(r => r.BranchId == user.BranchId && r.CreateDate > DateTime.Today)
                .ToListAsync();
            refundHomeVM.RefundOrders = await _context.RefundOrders
                .Include(ro => ro.Customer)
                .Where(ro => ro.BranchId == user.BranchId)
                .ToListAsync();
            return View(refundHomeVM);
        }
        [HttpGet]
        public async Task<IActionResult> RefundOrder(int? id, int? SaleId)
        {
            if (id == null || id == 0 || SaleId == null || SaleId == 0) return NotFound();
            Product? product = await _context.Products
                .Include(p=>p.ProductModel)
                .Include(p=>p.ProductCategory)
                .Include(p=>p.Brand)
                .Include(p=>p.Color)
                .FirstOrDefaultAsync(p => p.Id == id);
            Sale? sale = await _context.Sales
                .Include(s=>s.Customer)
                .Include(s=>s.Employee)
                .FirstOrDefaultAsync(s => s.Id == SaleId);
            if(sale == null || product == null) return NotFound();
            RefundOrderVM refundOrderVM = new();
            refundOrderVM.Product = product;
            refundOrderVM.Sale = sale;
            return View(refundOrderVM);
        }

        [HttpPost]
        public async Task<IActionResult> RefundOrder(RefundOrderVM refundOrderVM)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null || refundOrderVM == null) return NotFound();         
            if (!User.Identity.IsAuthenticated) return NotFound();
            Product? product = await _context.Products
               .Include(p => p.SaleProducts)
               .Include(p => p.ProductCategory)
               .Include(p => p.Color)
               .Include(p => p.Brand)
               .Include(p => p.ProductModel)
               .Where(p => p.BranchId == user.BranchId)
               .FirstOrDefaultAsync(p => p.Id == refundOrderVM.ProductId);
            Sale? sale = await _context.Sales
                .Include(s => s.SaleProducts)
                .Include(s => s.Employee)
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(s => s.Id == refundOrderVM.SaleId);
            if (product == null || !product.IsSold || sale == null) return NotFound();
            if (!ModelState.IsValid)
            {                            
                refundOrderVM.Product = product;
                refundOrderVM.Sale = sale;
                return View(refundOrderVM);
            }
           
            List<RefundOrder> refundOrder = await _context.RefundOrders.ToListAsync();
            if (refundOrder.Any(r => r.ProdId == product.Id)) return NotFound();
            bool check = false;
            foreach (var item in sale.SaleProducts)
            {
                if (item.ProductId == product.Id)
                {
                    check = true; break;
                }
            }
            if (!check) return NotFound();

            
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
            order.CustomerId = sale.CustomerId;
            order.EmployeeName = sale.Employee.FullName;
            order.BranchId = user.BranchId;
            order.SaleId = sale.Id;
            order.Description = refundOrderVM.Description;
            
            

            _context.RefundOrders.Add(order);
            await _context.SaveChangesAsync();
            TempData["Success"] = "ok";

            return RedirectToAction("details", "sale", new {id = sale.Id});
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
