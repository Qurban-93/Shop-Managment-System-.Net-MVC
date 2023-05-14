using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
      
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create() 
        {
            ViewBag.Brands = new SelectList(_context.Brands.ToList(),"Id","BrandName");
            ViewBag.ProductCategories = new SelectList(_context.ProductCategories.ToList(), "Id", "Name");
            ViewBag.Color = new SelectList(_context.Colors.ToList(), "Id", "ColorName");
            ViewBag.ProdModel = new SelectList(_context.ProductModels.ToList(), "Id", "ModelName");

            return View();
        }
    }
}
