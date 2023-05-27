using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels.CustomerVMs;
using System.Text.RegularExpressions;

namespace ShopManagmentSystem.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CustomerController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return View(await _context.Customers
                    .Where(c=>c.FullName.Contains(search.Trim().ToLower()) || c.Email.Contains(search.Trim().ToLower())).ToListAsync());
            }
            return View(_context.Customers.ToList());
        }

        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CustomerCreateVM customer)
        {
            if(!ModelState.IsValid)
            {
                return View(customer);
            }
            if(customer.PhoneNumber!=null)
            {
                string pattern = @"^(\+994)(50|51|55|70|77|10)(\d{7})$";
                if(!Regex.IsMatch(customer.PhoneNumber, pattern))
                {
                    ModelState.AddModelError("PhoneNumber", "Daxil etdiyiniz nomre duzgun formatda deil !");
                    return View(customer);
                }
            }
            
            if (_context.Customers.Any(p=>p.Email == customer.Email))
            {
                ModelState.AddModelError("Email","Bu Email sistemde movcuddur !");
                return View(customer);
            }
            if (_context.Customers.Any(c=>c.PhoneNumber == customer.PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Bu nomre sistemde movcuddur !");
                return View(customer);
            }
            Customer newCustomer = new Customer();
            newCustomer.FullName= customer.FullName;
            newCustomer.Email= customer.Email;
            newCustomer.Address= customer.Address;
            newCustomer.PhoneNumber= customer.PhoneNumber;
            newCustomer.TotalCost = 0;

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return BadRequest();
            if (id == null || id == 0) return NotFound();
            Customer? customer = await _context.Customers
                .Include(c=>c.Sales.Where(s=>s.BranchId== user.BranchId)).ThenInclude(s=>s.SaleProducts)
                .Include(c=>c.Refunds.Where(r=>r.BranchId == user.BranchId)).ThenInclude(r=>r.Product).ThenInclude(p=>p.ProductModel)
                .FirstOrDefaultAsync(c=>c.Id == id);
            if (customer == null) return NotFound();

            return View(customer);
        }
    }
}
