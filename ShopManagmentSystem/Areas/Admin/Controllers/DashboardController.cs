using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;
using System.Data;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DashboardController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles = "SuperAdmin")]  
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
