using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels.EmployeeVMs;
using System.Text.RegularExpressions;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public EmployeeController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            List<Employee> employees = _context.Employees
                .Include(e => e.EmployeePostion)
                .Include(e => e.Branch)
                .OrderBy(e=>e.BranchId)
                .Where(e=> !e.IsDeleted)
                .ToList();
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Position = new SelectList(_context.EmployeePostions.Where(ep=>!ep.IsDeleted).ToList(), "Id", "PositionName");
            ViewBag.Branch = new SelectList(_context.Branches.Where(b=>!b.IsDeleted).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateVM createVM)
        {
            ViewBag.Position = new SelectList(_context.EmployeePostions.Where(ep => !ep.IsDeleted).ToList(), "Id", "PositionName");
            ViewBag.Branch = new SelectList(_context.Branches.Where(b => !b.IsDeleted).ToList(), "Id", "Name");
            if (!ModelState.IsValid) return View();
            if(_context.Employees.Any(e=>e.FullName.Trim().ToLower()== createVM.FullName.Trim().ToLower()))
            {
                ModelState.AddModelError("FullName", "Bu adla employee movcuddur !Elave melumat daxil edin !");
                return View();
            }
            string pattern = @"^(\+994)(50|51|55|70|77|10|99)(\d{7})$";
            if (!Regex.IsMatch(createVM.Number, pattern))
            {
                ModelState.AddModelError("Number", "Daxil etdiyiniz nomre duzgun formatda deil !");
                return View(createVM);
            }
            if(_context.Employees.Any(e=>e.Number == createVM.Number))
            {
                ModelState.AddModelError("Number","Bu nomre artiq bazada var !");
                return View(createVM);
            }
            if(createVM.BirthDate > DateTime.Now)
            {
                ModelState.AddModelError("BirthDate", "Duzgun tarix secilmiyib !");
                return View(createVM);
            }

            Employee newEmployee = new();
            newEmployee.Number = createVM.Number;
            newEmployee.BirthDate= createVM.BirthDate;
            newEmployee.CreateDate = DateTime.Now;
            newEmployee.BranchId = createVM.BranchId;
            newEmployee.EmployeePostionId = createVM.EmployeePositionId;
            newEmployee.FullName = createVM.FullName;

            _context.Employees.Add(newEmployee);
            await _context.SaveChangesAsync();
            TempData["Create"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Position = new SelectList(_context.EmployeePostions.Where(ep => !ep.IsDeleted).ToList(), "Id", "PositionName");
            ViewBag.Branch = new SelectList(_context.Branches.Where(b => !b.IsDeleted).ToList(), "Id", "Name");
            if (id == 0 || id == null) return NotFound();
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(e=>e.Id == id && !e.IsDeleted);
            if (employee == null) return NotFound();
            EmployeeEditVM editVM = new()
            {
                FullName = employee.FullName,
                Number = employee.Number,
                BirthDate = employee.BirthDate, 
                BranchId = (int)employee.BranchId,
                EmployeePositionId = employee.EmployeePostionId

            };
            return View(editVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,EmployeeEditVM editVM)
        {
            ViewBag.Position = new SelectList(_context.EmployeePostions.Where(ep => !ep.IsDeleted).ToList(), "Id", "PositionName");
            ViewBag.Branch = new SelectList(_context.Branches.Where(b => !b.IsDeleted).ToList(), "Id", "Name");
            if (!ModelState.IsValid) return View();
            if(id == 0 || id == null) return NotFound(); 
            if(_context.Employees.Any(e=>e.FullName.Trim().ToLower() == editVM.FullName.Trim().ToLower() && e.Id != id))
            {
                ModelState.AddModelError("FullName", "Bu adla employee movcuddur! Elave melumat daxil edin.");
                return View();
            }
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(e=>e.Id == id && !e.IsDeleted);
            if (employee == null) return NotFound();
            employee.UpdateDate = DateTime.Now;
            employee.FullName= editVM.FullName;
            employee.Number = editVM.Number;
            employee.BirthDate = editVM.BirthDate;
            employee.BranchId = editVM.BranchId;
            employee.EmployeePostionId = editVM.EmployeePositionId;
            await _context.SaveChangesAsync();
            TempData["Edit"] = true;

            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(e=>e.Id == id && !e.IsDeleted);
            if (employee == null) return NotFound();
            employee.IsDeleted= true;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
