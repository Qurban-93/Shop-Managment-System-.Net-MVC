using Microsoft.AspNetCore.Mvc;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            SettingVM settingVM = new()
            {
                BrandCount= _context.Brands.Count(),
                ColorsCount= _context.Colors.Count(),
                EmployeeCount= _context.Employees.Count(),
                EmployeePositionCount= _context.EmployeePostions.Count(),
                ProductModelCount= _context.ProductModels.Count(),
                ProductsCategoryCount = _context.ProductCategories.Count(),
            };
            return View(settingVM);
        }
    }
}
