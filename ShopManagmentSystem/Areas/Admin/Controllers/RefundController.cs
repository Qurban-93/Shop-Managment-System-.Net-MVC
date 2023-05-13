using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Areas.Admin.Controllers;

[Area("Admin")]
//[Authorize(Roles = "Admin,SuperAdmin")]
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
        AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
        List<RefundOrder> Orders = await _context.RefundOrders
            .Include(o => o.Customer)
            .Where(o => o.BranchId == user.BranchId).ToListAsync();
        return View(Orders);
    }

    public async Task<IActionResult> Orders(int? id)
    {
        AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (id == null || id == 0) return NotFound();
        RefundOrder? refundOrder =await _context.RefundOrders
            .Include(o => o.Customer)
            .Where(o => o.BranchId == user.BranchId).FirstOrDefaultAsync(o => o.Id == id);
        if (refundOrder == null) return NotFound();

        RefundVM refundVM = new RefundVM();
        refundVM.Order = refundOrder;
        refundVM.RefundOrderId = refundOrder.Id;
        return View(refundVM);
    }
    [HttpPost]
    public async Task<IActionResult> Orders(RefundVM refundVM)
    {
        AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (refundVM == null) return NotFound();      
        RefundOrder? refundOrder = await _context.RefundOrders.FirstOrDefaultAsync(ro => ro.Id == refundVM.RefundOrderId);
        Customer? customer = await _context.Customers.FirstOrDefaultAsync(c=>c.Id == refundOrder.CustomerId);
        Product? product = await _context.Products.Include(p=>p.ProductModel).Include(p=>p.ProductCategory).FirstOrDefaultAsync(p => p.Id == refundOrder.ProdId);
        Employee? employee = await _context.Employees.FirstOrDefaultAsync(e => e.FullName == refundOrder.EmployeeName);
        if (refundOrder == null || customer == null || product == null || employee == null) return NotFound();
        Refund refund = new();
        refund.CreateDate = DateTime.Now;
        refund.ProductId = product.Id;
        refund.SaleId = refundOrder.SaleId;
        refund.BranchId = user.BranchId;
        refund.CustomerId = refundOrder.CustomerId;
        refund.Discount = refundVM.Discount;
        refund.CashlessPayment = refundVM.CashlessPayment;
        refund.EmployeeId = employee.Id;
        refund.TotalPrice = product.ProductModel.ModelPrice - refundVM.Discount;
        refund.TotalLoss = (product.ProductModel.ModelPrice - refundVM.Discount) - product.CostPrice;
        product.IsSold = false;
        customer.TotalCost = customer.TotalCost - refund.TotalPrice;
        Salary salary = new();
        salary.CreateDate = DateTime.Now;
        salary.Bonus = 0 - product.ProductCategory.Bonus;
        salary.EmployeeId = employee.Id;
        
        _context.Salaries.Add(salary);
        _context.Refunds.Add(refund);
        _context.RefundOrders.Remove(refundOrder);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpDelete]
    public async Task<IActionResult> OrderDelete(int? id)
    {
        if (id == null || id == 0) return NotFound();
        RefundOrder? refundOrder = await _context.RefundOrders.FirstOrDefaultAsync(x => x.Id == id);
        if (refundOrder == null) return NotFound();
        _context.RefundOrders.Remove(refundOrder);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
