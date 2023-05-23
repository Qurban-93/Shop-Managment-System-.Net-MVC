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
                EmployeeId = createVM.EmployeeId,
                Bonus = 0 - createVM.Amount,
                Punishment= punishment,
                CreateDate= DateTime.Now,

            };

            await _context.Salaries.AddAsync(salary);
            await _context.Punishment.AddAsync(punishment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Edit(int? id)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Employees = new SelectList(await _context.Employees.Where(e => e.BranchId == user.BranchId).ToListAsync(), "Id", "FullName");

            if (id == null || id == 0) return NotFound();
            Punishment? punishment = await _context.Punishment.FindAsync(id);
            if (punishment == null) return NotFound();
            PunishmentEditVM editVM = new()
            {
                Amount = punishment.Amount,
                Descpription = punishment.Descpription,
                EmployeeId =punishment.EmployeeId,
            };
            return View(editVM);
        }
        public async Task<IActionResult> Edit(PunishmentEditVM editVM,int? id)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Employees = new SelectList(await _context.Employees.Where(e => e.BranchId == user.BranchId).ToListAsync(), "Id", "FullName");

            if (!ModelState.IsValid) return NotFound();
            if(id == 0 || id == null) return NotFound();
            Punishment? existPunishment = await _context.Punishment.FindAsync(id);
            if (existPunishment == null) return NotFound();
            existPunishment.UpdateDate = DateTime.Now;
            existPunishment.Descpription = editVM.Descpription;
            existPunishment.Amount = editVM.Amount;
            existPunishment.EmployeeId= editVM.EmployeeId;
            Salary? salary = await _context.Salaries.FirstOrDefaultAsync(s => s.PunishmentId == existPunishment.Id);
            if (salary == null) return NotFound();
            salary.EmployeeId = existPunishment.EmployeeId;
            salary.UpdateDate = DateTime.Now;
            salary.Bonus = 0 - existPunishment.Amount;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Punishment? punishment = await _context.Punishment.FindAsync(id);
            if (punishment == null) return BadRequest();
            Salary? salary = await _context.Salaries.FirstOrDefaultAsync(s=>s.PunishmentId ==punishment.Id);
            if (salary == null) return NotFound();
            _context.Salaries.Remove(salary);
            _context.Punishment.Remove(punishment);
            await _context.SaveChangesAsync();

            return Ok(punishment);
        }
    }
}
