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
            SettingsVM settingsVM = new();
            settingsVM.Brands = _context.Brands.ToList();
            return View(settingsVM);
        }
    }
}
