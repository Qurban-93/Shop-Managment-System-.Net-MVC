using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;
using System.Data;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        

        public DashboardController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            var profitAmount = _context.Sales.Where(s=>s.CreateDate > DateTime.Today).Sum(s=>s.TotalProfit);
            var saleAmount = _context.Sales.Where(s => s.CreateDate > DateTime.Today).Sum(s => s.TotalPrice);
            var productAmount = _context.Sales.Where(s => s.CreateDate > DateTime.Today).Include(s=>s.SaleProducts).ToList();
            var refundsAmount = _context.Refunds.Where(s => s.CreateDate > DateTime.Today).Sum(s => s.TotalPrice);
            var salaryAmount = _context.Salaries.Where(s => s.CreateDate > DateTime.Today && s.Employee.BranchId == user.BranchId).Sum(s => s.Bonus);
            AdminIndexVM indexVM = new()
            {
                ProductAmount = productAmount.Sum(e=>e.SaleProducts.Count()),
                SalaryAmount = salaryAmount,
                SaleAmount = saleAmount,
                RefundsAmount = refundsAmount,
                ProfitAmount = profitAmount,
            };
            
            return View(indexVM);
        }

        [Authorize(Roles = "MainAdmin")]  
        public IActionResult SuperAdmin()
        {
            SettingVM settingVM = new()
            {
                BranchCount = _context.Branches.Count(),
                BrandCount = _context.Brands.Count(),
                ColorsCount = _context.Colors.Count(),
                EmployeeCount = _context.Employees.Count(),
                EmployeePositionCount = _context.EmployeePostions.Count(),
                ProductModelCount = _context.ProductModels.Count(),
                ProductsCategoryCount = _context.ProductCategories.Count(),
                UserCount = _userManager.Users.Count(),
            };
            return View(settingVM);
        }
    }
}
