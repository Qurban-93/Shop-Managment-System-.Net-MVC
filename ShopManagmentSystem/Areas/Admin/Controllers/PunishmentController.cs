using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PunishmentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public PunishmentController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string search, DateTime? fromDate, DateTime? toDate)
        {
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.SearchValue = search;

            if (!string.IsNullOrEmpty(search))
            {
                if (toDate > fromDate)
                {
                    ViewBag.Error = "Error Data";
                    return View(await _context.Punishment
                    .Include(p => p.Employee).Where(p => p.Employee.FullName.Trim().ToLower().Contains(search.Trim().ToLower()))
                    .ToListAsync());
                }
                if (toDate != null && fromDate == null)
                {
                    return View(await _context.Punishment
                     .Include(p => p.Employee).Where(p => p.CreateDate < toDate.Value.AddHours(23) &&
                     p.Employee.FullName.Trim().ToLower().Contains(search.Trim().ToLower()))
                     .ToListAsync());
                }
                if (fromDate != null && toDate == null)
                {
                    return View(await _context.Punishment
                    .Include(p => p.Employee).Where(p => p.CreateDate > fromDate &&
                    p.Employee.FullName.Trim().ToLower().Contains(search.Trim().ToLower()))
                    .ToListAsync());
                }
                if (fromDate != null && toDate != null)
                {
                    return View(await _context.Punishment
                    .Include(p => p.Employee).Where(p => p.CreateDate > fromDate && p.CreateDate < toDate.Value.AddHours(23)
                    && p.Employee.FullName.Trim().ToLower().Contains(search.Trim().ToLower())).ToListAsync());

                }
                if (fromDate == null && toDate == null)
                {
                    ViewBag.FromDate = DateTime.Today;
                    ViewBag.ToDate = DateTime.Today.AddHours(23);
                    return View(await _context.Punishment
                    .Include(p => p.Employee)
                    .ToListAsync());
                }

            }

            if (toDate > fromDate)
            {
                ViewBag.Error = "Error Data";
                return View(await _context.Punishment
                .Include(p => p.Employee).ToListAsync());
            }
            if (toDate != null && fromDate == null)
            {
                return View(await _context.Punishment
                .Include(p => p.Employee).Where(p => p.CreateDate < toDate.Value.AddHours(23)).ToListAsync());
            }
            if (fromDate != null && toDate == null)
            {
                return View(await _context.Punishment
                .Include(p => p.Employee).Where(p => p.CreateDate > fromDate)
                .ToListAsync());
            }
            if (fromDate != null && toDate != null)
            {
                return View(await _context.Punishment
                .Include(p => p.Employee).Where(p => p.CreateDate > fromDate).ToListAsync());

            }

            ViewBag.FromDate = DateTime.Today;
            ViewBag.ToDate = DateTime.Today.AddHours(23);

            return View(await _context.Punishment
                .Include(p => p.Employee)
                .ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            ViewBag.Employees = new SelectList(await _context.Employees.Where(e => e.BranchId == user.BranchId).ToListAsync(), "Id", "FullName");
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(PunishmentCreateVM createVM)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            ViewBag.Employees = new SelectList(await _context.Employees.Where(e => e.BranchId == user.BranchId).ToListAsync(), "Id", "FullName");
            if (!ModelState.IsValid) return View();
            Punishment punishment = new()
            {
                Amount = createVM.Amount,
                Descpription = createVM.Descpription,
                EmployeeId = createVM.EmployeeId,
                CreateDate = DateTime.Now,
            };

            Salary salary = new()
            {
                Bonus = 0 - createVM.Amount,

            };

            await _context.Punishment.AddAsync(punishment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
    }
}
