using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Extensions;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels.ProductVMs;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }
     
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "BrandName");
            ViewBag.ProductCategories = new SelectList(_context.ProductCategories.ToList(), "Id", "Name");
            ViewBag.Color = new SelectList(_context.Colors.ToList(), "Id", "ColorName");
            ViewBag.ProdModel = new SelectList(_context.ProductModels.ToList(), "Id", "ModelName");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "BrandName");
            ViewBag.ProductCategories = new SelectList(_context.ProductCategories.ToList(), "Id", "Name");
            ViewBag.Color = new SelectList(_context.Colors.ToList(), "Id", "ColorName");
            ViewBag.ProdModel = new SelectList(_context.ProductModels.ToList(), "Id", "ModelName");
            if (!ModelState.IsValid) return View();
            if (_context.Products.Any(p => p.Series == productCreateVM.Series))
            {
                ModelState.AddModelError("Series", "Bu seriyali mehsul artiq movcuddur !");
                return View();
            }
            Brand? brand = await _context.Brands.Include(b => b.ProductModels).FirstOrDefaultAsync(b => b.Id == productCreateVM.BrandId);
            ProductCategory? productCategory = await _context.ProductCategories.Include(pc => pc.ProductModels).FirstOrDefaultAsync(pc => pc.Id == productCreateVM.ProductCategoryId);
            ProductModel? productModel = await _context.ProductModels.FirstOrDefaultAsync(pm => pm.Id == productCreateVM.ProductModelId);
            Color? color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == productCreateVM.ColorId);
            if (productModel == null || brand == null || productCategory == null || color == null) return NotFound();
            if (!brand.ProductModels.Any(pm => pm.Id == productModel.Id) || !productCategory.ProductModels.Any(pm => pm.Id == productModel.Id))
            {
                ModelState.AddModelError("ProductModelId", "Qeyd etdiyiniz model categoriya ve ya brende aid deil !");
                return View();
            }
            if (productCreateVM.CostPrice > productModel.ModelPrice)
            {
                ModelState.AddModelError("CostPrice", "Cost Price Satish giymetinden cox ola bilmez !");
                return View();
            }
            if(productCreateVM.CostPrice == 0)
            {
                ModelState.AddModelError("CostPrice", "Deyer '0' ola bilmez !");
                return View();
            }           

            Product product = new();            
            product.ProductModelId = productCreateVM.ProductModelId;
            product.ProductCategoryId = productCreateVM.ProductCategoryId;
            product.ColorId = productCreateVM.ColorId;
            product.BrandId = productCreateVM.BrandId;
            product.BranchId = user.BranchId;
            product.CostPrice = productCreateVM.CostPrice;
            product.CreateDate = DateTime.Now;
            product.Series = productCreateVM.Series;
            product.IsSold = false;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
