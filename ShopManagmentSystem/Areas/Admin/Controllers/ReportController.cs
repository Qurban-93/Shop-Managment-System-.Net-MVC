using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.Service;
using ShopManagmentSystem.ViewModels;
using ShopManagmentSystem.ViewModels.SalaryVMs;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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

        public async Task<IActionResult> Salary(DateTime? fromDate,DateTime? toDate)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            ViewBag.Fromdate = fromDate;
            ViewBag.ToDate = toDate;
            SalaryVM salaryVM = new SalaryVM();
           
            salaryVM.Employees  = await _context.Employees
                .Include(e => e.EmployeePostion)
                .Include(e => e.Salaries)
                .Where(e => e.BranchId == user.BranchId)
                .ToListAsync(); ;

            if (fromDate > toDate)
            {
                ViewBag.Error = "Invalid Date Time";
                salaryVM.Salaries = await _context.Salaries
               .Include(s => s.Sale)
               .Include(s => s.Employee)
               .Include(s => s.Refund)
               .Where(s => s.CreateDate > DateTime.Today)
               .ToListAsync();
                return View(salaryVM);
              
            }
            if (fromDate != null && toDate == null)
            {
                 salaryVM.Salaries = await _context.Salaries
                .Include(s => s.Sale)
                .Include(s => s.Employee)
                .Include(s => s.Refund)
                .Where(s=>s.CreateDate > fromDate)
                .ToListAsync();
                return View(salaryVM);
            }
            if (fromDate == null && toDate != null)
            {
                salaryVM.Salaries = await _context.Salaries
                .Include(s => s.Sale)
                .Include(s => s.Employee)
                .Include(s => s.Refund)
                .Where(s => s.CreateDate < toDate)
                .ToListAsync();
                return View(salaryVM);
            }
            if (fromDate != null && toDate != null)
            {
                toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);              
                salaryVM.Salaries = await _context.Salaries
                .Include(s => s.Sale)
                .Include(s => s.Employee)
                .Include(s => s.Refund)
                .Where(s => s.CreateDate < toDate && s.CreateDate > fromDate)
                .ToListAsync();
                return View(salaryVM);
            }
            if (fromDate == null && toDate == null)
            {
                ViewBag.fromDate = DateTime.Today;
                ViewBag.toDate = DateTime.Today;
                salaryVM.Salaries = await _context.Salaries
               .Include(s => s.Sale)
               .Include(s => s.Employee)
               .Include(s => s.Refund)
               .Where(s => s.CreateDate > DateTime.Today)
               .ToListAsync();

            }              
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
                List<ProfitVM> profitVMs= new();
                foreach (var item in _context.Sales.Where(s=>s.BranchId == user.BranchId).ToList())
                {
                    ProfitVM profitVM = new();
                    profitVM.Date = DateTime.Now;
                    profitVM.Profit = item.TotalProfit;
                    profitVMs.Add(profitVM);
                }

                foreach(var item in _context.Refunds.Where(s => s.BranchId == user.BranchId).ToList())
                {
                    ProfitVM profitVM = new();
                    profitVM.Date = DateTime.Now;
                    profitVM.Profit = 0 - item.TotalLoss;
                    profitVMs.Add(profitVM);
                }

                return View(profitVMs.OrderBy(p=>p.Date));
            }
            if (fromDate != null && toDate == null)
            {
                List<ProfitVM> profitVMs = new();
                foreach (var item in _context.Sales.Where(s=>s.CreateDate > fromDate && s.BranchId == user.BranchId).ToList())
                {
                    ProfitVM profitVM = new();
                    profitVM.Date = DateTime.Now;
                    profitVM.Profit = item.TotalProfit;
                    profitVMs.Add(profitVM);
                }

                foreach (var item in _context.Refunds.Where(r => r.CreateDate > fromDate && r.BranchId == user.BranchId).ToList())
                {
                    ProfitVM profitVM = new();
                    profitVM.Date = DateTime.Now;
                    profitVM.Profit = 0 - item.TotalLoss;
                    profitVMs.Add(profitVM);
                }
                return View(profitVMs.OrderBy(p=>p.Date));
            }
            if (fromDate == null && toDate != null)
            {
                List<ProfitVM> profitVMs = new();
                foreach (var item in _context.Sales.Where(s => s.CreateDate < toDate && s.BranchId == user.BranchId).ToList())
                {
                    ProfitVM profitVM = new();
                    profitVM.Date = DateTime.Now;
                    profitVM.Profit = item.TotalProfit;
                    profitVMs.Add(profitVM);
                }

                foreach (var item in _context.Refunds.Where(r => r.CreateDate < toDate && r.BranchId == user.BranchId).ToList())
                {
                    ProfitVM profitVM = new();
                    profitVM.Date = DateTime.Now;
                    profitVM.Profit = 0 - item.TotalLoss;
                    profitVMs.Add(profitVM);
                }
                return View(profitVMs.OrderBy(p => p.Date));
            }
            if (fromDate != null && toDate != null)
            {
                toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                List<ProfitVM> profitVMs = new();
                foreach (var item in _context.Sales.Where(s => s.CreateDate > fromDate && s.CreateDate <toDate && s.BranchId == user.BranchId).ToList())
                {
                    ProfitVM profitVM = new();
                    profitVM.Date = DateTime.Now;
                    profitVM.Profit = item.TotalProfit;
                    profitVMs.Add(profitVM);
                }

                foreach (var item in _context.Refunds.Where(r => r.CreateDate > fromDate && r.CreateDate < toDate && r.BranchId == user.BranchId).ToList())
                {
                    ProfitVM profitVM = new();
                    profitVM.Date = DateTime.Now;
                    profitVM.Profit = 0 - item.TotalLoss;
                    profitVMs.Add(profitVM);
                }
                return View(profitVMs.OrderBy(p => p.Date));              
            }        
            if (fromDate == null && toDate == null)
            {
                ViewBag.fromDate = DateTime.Today;
                ViewBag.toDate = DateTime.Today.AddHours(23);
                List<ProfitVM> profitVMs = new();
                List<Sale> sales = await _context.Sales.Where(s => s.CreateDate > DateTime.Today && s.BranchId == user.BranchId).ToListAsync();
                List<Refund> refunds = await _context.Refunds.Where(r => r.CreateDate > DateTime.Today && r.BranchId == user.BranchId).ToListAsync();
                foreach (var item in sales)
                {
                    ProfitVM profitVM = new();
                    profitVM.Date = DateTime.Now;
                    profitVM.Profit = item.TotalProfit;
                    profitVMs.Add(profitVM);
                }

                foreach (var item in refunds)
                {
                    ProfitVM profitVM = new();
                    profitVM.Date = DateTime.Now;
                    profitVM.Profit = 0 - item.TotalLoss;
                    profitVMs.Add(profitVM);
                }
                return View(profitVMs.OrderBy(p => p.Date));
            }  
            return NotFound();
        }
    }
}
