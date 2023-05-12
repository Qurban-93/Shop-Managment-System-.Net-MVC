using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ReportController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Salary()
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<Employee> employees = await _context.Employees
                .Include(e=>e.Salaries)
                .Include(e=>e.EmployeePostion)
                .Where(e=>e.BranchId == user.BranchId)
                .ToListAsync();
            return View(employees);
        }

        public IActionResult Profit()
        {

            return View();
        }
    }
}
