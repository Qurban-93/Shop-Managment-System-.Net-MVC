using Microsoft.AspNetCore.Mvc;
using ShopManagmentSystem.DAL;

namespace ShopManagmentSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Customers.ToList());
        }
    }
}
