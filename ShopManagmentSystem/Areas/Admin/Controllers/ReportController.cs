using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.Service;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBoxOfficeService _boxOfficeService;

        public ReportController(AppDbContext context, UserManager<AppUser> userManager, IBoxOfficeService boxOfficeService)
        {
            _context = context;
            _userManager = userManager;
            _boxOfficeService = boxOfficeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Salary(DateTime? fromdate,DateTime? toDate)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
           List<Salary> salaryList = await _context.Salaries
                .Include(s=>s.Sale)
                .Include(s=>s.Employee)
                .Include(s=>s.Refund)
                .ToListAsync();
            List<Employee> employees = await _context.Employees
                .Include(e=>e.EmployeePostion)
                .Include(e=>e.Salaries)
                .Where(e=>e.BranchId == user.BranchId)
                .ToListAsync();

            SalaryVM salaryVM = new SalaryVM(); 
            salaryVM.Salaries = salaryList;
            salaryVM.Employees = employees;
            return View(salaryVM);
        }

        public async Task<IActionResult> Profit(DateTime? fromDate, DateTime? toDate)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("login", "account");
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return BadRequest();          
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;

            if (fromDate > toDate)
            {
                ViewBag.Error = "Invalid Date Time";

                return View(_boxOfficeService.GetAll(user));
            }
            if (fromDate != null && toDate == null)
            {
                return View(_boxOfficeService.GetAll((DateTime)fromDate, user));
            }
            if (fromDate == null && toDate != null)
            {
                toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);

                return View(_boxOfficeService.GetAll(user, (DateTime)toDate));
            }
            if (fromDate != null && toDate != null)
            {
                toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                return View(_boxOfficeService.GetAll((DateTime)fromDate, user, (DateTime)toDate));
            }

            List<BoxOfficeVM> boxOfficeVMs = _boxOfficeService.GetAll(user);


            return View(boxOfficeVMs);
        }
    }
}
