using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels.EmployeeVMs;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "MainAdmin")]
    public class EmployeePositionController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeePositionController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.EmployeePostions.Where(ep => !ep.IsDeleted).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeePositionCreateVM createVM)
        {
            if (!ModelState.IsValid) return View(createVM);
            if (_context.EmployeePostions.Any(ep => ep.PositionName.Trim().ToLower() == createVM.PositionName.Trim().ToLower() && !ep.IsDeleted))
            {
                ModelState.AddModelError("PositionName", "Bu adli Position movcuddur !");
                return View();
            }
            if (createVM.FixSalary < 300)
            {
                ModelState.AddModelError("FixSalary", "Minimum emeq haqqindan ashaqi olmaz !");
                return View();
            }
            EmployeePosition employeePosition = new();
            employeePosition.FixSalary = createVM.FixSalary;
            employeePosition.PositionName = createVM.PositionName;
            employeePosition.CreateDate = DateTime.Now;

            _context.EmployeePostions.Add(employeePosition);
            await _context.SaveChangesAsync();
            TempData["Create"] = true;

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == 0 || id == null) return NotFound();
            EmployeePosition? employeePosition = await _context.EmployeePostions.FirstOrDefaultAsync(ep => ep.Id == id && !ep.IsDeleted);
            if (employeePosition == null) return NotFound();
            EmployeePositionEditVM editVM = new()
            {
                PositionName = employeePosition.PositionName,
                FixSalary = (double)employeePosition.FixSalary
            };
            return View(editVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, EmployeePositionEditVM editVM)
        {
            if (id == null || id == 0) return NotFound();
            if (!ModelState.IsValid) return View();
            if (_context.EmployeePostions.Any(ep => ep.PositionName.Trim().ToLower() == editVM.PositionName.Trim().ToLower() && ep.Id != id && !ep.IsDeleted))
            {
                ModelState.AddModelError("PositionName", "Bu adli position movcuddur!");
                return View();
            }
            EmployeePosition? employeePosition = await _context.EmployeePostions.FirstOrDefaultAsync(ep => ep.Id == id && !ep.IsDeleted);
            if (employeePosition == null) return NotFound();
            employeePosition.UpdateDate = DateTime.Now;
            employeePosition.PositionName = editVM.PositionName;
            employeePosition.FixSalary = editVM.FixSalary;
            await _context.SaveChangesAsync();
            TempData["Edit"] = true;
            return RedirectToAction(nameof(Index));

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0 || id == null) return NotFound();
            EmployeePosition? existEmployeePosition = await _context.EmployeePostions.FirstOrDefaultAsync(ep => ep.Id == id && !ep.IsDeleted);
            if (existEmployeePosition == null) return NotFound();
            existEmployeePosition.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
