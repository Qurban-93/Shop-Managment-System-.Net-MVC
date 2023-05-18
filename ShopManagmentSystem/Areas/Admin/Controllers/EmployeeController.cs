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
                .ToList();
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Position = new SelectList(_context.EmployeePostions.ToList(), "Id", "PositionName");
            ViewBag.Branch = new SelectList(_context.Branches.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateVM createVM)
        {
            ViewBag.Position = new SelectList(_context.EmployeePostions.ToList(), "Id", "PositionName");
            ViewBag.Branch = new SelectList(_context.Branches.ToList(), "Id", "Name");
            if (!ModelState.IsValid) return View();
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
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(e=>e.Id == id);
            if (employee == null) return NotFound();
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
