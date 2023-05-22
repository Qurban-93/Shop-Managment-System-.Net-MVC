using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PunishmentController : Controller
    {
        private readonly AppDbContext _context;

        public PunishmentController(AppDbContext context)
        {
            _context = context;
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
    }
}
