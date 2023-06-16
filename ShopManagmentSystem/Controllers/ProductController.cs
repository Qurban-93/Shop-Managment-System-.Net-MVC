using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? search)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            List<Product> products = await _appDbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductModel)
                .Include(p => p.Brand)
                .Include(p => p.Color)
                .Where(p => !p.IsSold && p.BranchId == user.BranchId && !p.IsDeleted).ToListAsync();

            

            List<Order> orders = await _appDbContext.Orders.Where(o => o.BranchId == user.BranchId)
                .ToListAsync();

            if (orders != null || orders.Count > 0)
            {
                foreach (var item in orders)
                {
                    if (products.Any(p => p.Id == item.ProdId))
                    {
                        products.Remove(products.FirstOrDefault(p => p.Id == item.ProdId));
                    }
                }

            }
            ViewBag.SearchValue = search;

            return View(products);
        }

        public async Task<IActionResult> Search(string search)
        {
            List<Product> products;
            if (string.IsNullOrWhiteSpace(search))
            {
                products = new();
            }
            else
            {
                products = await _appDbContext.Products
               .Include(p => p.Brand)
               .Include(p => p.ProductModel)
               .Include(p => p.Color)
               .Where(p => !p.IsSold && (p.ProductModel.ModelName.ToLower().Trim().Contains(search.ToLower().Trim()) ||
               p.Brand.BrandName.ToLower().Trim().Contains(search.ToLower().Trim())) && !p.IsDeleted)
               .ToListAsync();
            }
            List<Branch> branches = await _appDbContext.Branches.Where(b => b.Id != 5 && !b.IsDeleted).ToListAsync();
            SearchVM searchVM = new()
            {
                Products = products,
                Branches = branches
            };
            ViewBag.SearchValue = search;
            return View(searchVM);
        }
    }
}
