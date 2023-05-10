using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;
using System.Text.RegularExpressions;

namespace ShopManagmentSystem.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return View(_context.Customers
                    .Where(c=>c.FullName.Contains(search.Trim().ToLower()) 
                || c.Email.Contains(search.Trim().ToLower())));
            }
            return View(_context.Customers.ToList());
        }

        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
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
    }
}
