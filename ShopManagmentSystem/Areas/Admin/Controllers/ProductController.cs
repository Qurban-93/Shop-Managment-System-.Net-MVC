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

        public async Task<IActionResult> Index(string search)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if(user == null) return NotFound();

            List<Product> products =await _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductModel)
                .Include(p => p.Brand)
                .Include(p => p.Color)
                .Include(p=>p.Memory)
                .Where(p=>p.BranchId == user.BranchId).ToListAsync();
                
            return View(products);
        } 
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "BrandName");         
            ViewBag.Color = new SelectList(_context.Colors.ToList(), "Id", "ColorName");
            ViewBag.ProdModel = new SelectList(_context.ProductModels.ToList(), "Id", "ModelName");
            ViewBag.Memory = new SelectList(_context.Memories.ToList(), "Id", "MemoryCapacity");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "BrandName");            
            ViewBag.Color = new SelectList(_context.Colors.ToList(), "Id", "ColorName");
            ViewBag.ProdModel = new SelectList(_context.ProductModels.ToList(), "Id", "ModelName");
            ViewBag.Memory = new SelectList(_context.Memories.ToList(), "Id", "MemoryCapacity");
            if (!ModelState.IsValid) return View();
            if (_context.Products.Any(p => p.Series == productCreateVM.Series))
            {
                ModelState.AddModelError("Series", "Bu seriya artiq movcuddur !");
                return View();
            }
            Brand? brand = await _context.Brands.Include(b => b.ProductModels).FirstOrDefaultAsync(b => b.Id == productCreateVM.BrandId);
            ProductModel? productModel = await _context.ProductModels.FirstOrDefaultAsync(pm => pm.Id == productCreateVM.ProductModelId);
            Color? color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == productCreateVM.ColorId);
            Memory memory = await _context.Memories.FirstOrDefaultAsync(m=>m.Id == productCreateVM.MemoryId);
            if (productModel == null || brand == null  || color == null || memory == null) return NotFound();
            if (!brand.ProductModels.Any(pm => pm.Id == productModel.Id))
            {
                ModelState.AddModelError("ProductModelId", "Qeyd etdiyiniz model secdiyiniz Brende aid deil !");
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
            product.ProductCategoryId = (int)productModel.ProductCategoryId;
            product.ColorId = productCreateVM.ColorId;
            product.BrandId = productCreateVM.BrandId;
            product.BranchId = user.BranchId;
            product.CostPrice = productCreateVM.CostPrice;
            product.CreateDate = DateTime.Now;
            product.Series = productCreateVM.Series;
            product.IsSold = false;
            product.MemoryId = productCreateVM.MemoryId;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            TempData["Create"] = true;

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "BrandName");
            ViewBag.Memory = new SelectList(_context.Memories.ToList(), "Id", "MemoryCapacity");
            ViewBag.Color = new SelectList(_context.Colors.ToList(), "Id", "ColorName");
            ViewBag.ProdModel = new SelectList(_context.ProductModels.ToList(), "Id", "ModelName");
            if (id == null || id == 0) return NotFound();
            Product? product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return NotFound();
            ProductEditVM editVM = new()
            {
                CostPrice = product.CostPrice,
                BrandId= product.BrandId,
                ColorId = product.ColorId,              
                ProductModelId = product.ProductModelId,
                Series = product.Series,
                MemoryId = product.MemoryId,
            };

            return View(editVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id ,ProductEditVM editVM)
        {
            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "BrandName");
            ViewBag.Memory = new SelectList(_context.Memories.ToList(), "Id", "MemoryCapacity");
            ViewBag.Color = new SelectList(_context.Colors.ToList(), "Id", "ColorName");
            ViewBag.ProdModel = new SelectList(_context.ProductModels.ToList(), "Id", "ModelName");
            if (!ModelState.IsValid) return View();
            if(id == 0 || id == null ) return NotFound();
            Product product = await _context.Products.FirstAsync(x => x.Id == id);
            if (product == null ) return NotFound();
            if(await _context.Products.AnyAsync(p=>p.Series == editVM.Series && p.Id != id))
            {
                ModelState.AddModelError("Series", "Bu seriya movcuddur !");
                return View();
            }
            Brand? brand = await _context.Brands.Include(b => b.ProductModels).FirstOrDefaultAsync(b => b.Id == editVM.BrandId);
            ProductModel? productModel = await _context.ProductModels.FirstOrDefaultAsync(pm => pm.Id == editVM.ProductModelId);
            Color? color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == editVM.ColorId);
            Memory? memory = await _context.Memories.FirstOrDefaultAsync( m =>m.Id == editVM.MemoryId);
            if (productModel == null || brand == null || color == null || memory == null) return NotFound();
            if (!brand.ProductModels.Any(pm => pm.Id == productModel.Id))
            {
                ModelState.AddModelError("ProductModelId", "Qeyd etdiyiniz model secidiyinz brende aid deil !");
                return View();
            }
            if (editVM.CostPrice > productModel.ModelPrice)
            {
                ModelState.AddModelError("CostPrice", "Cost Price Satish giymetinden cox ola bilmez !");
                return View();
            }
            if (editVM.CostPrice == 0)
            {
                ModelState.AddModelError("CostPrice", "Deyer '0' ola bilmez !");
                return View();
            }

            product.BrandId = brand.Id;
            product.ProductCategoryId = (int)productModel.ProductCategoryId;
            product.ProductModelId = productModel.Id;
            product.ColorId = color.Id;
            product.CostPrice = editVM.CostPrice;
            product.Series = editVM.Series;
            product.UpdateDate = DateTime.Now;
            product.ProductCategoryId = (int)productModel.ProductCategoryId;
            product.MemoryId = editVM.MemoryId;

            await _context.SaveChangesAsync();
            TempData["Edit"] = true;
            return RedirectToAction("Index");
        }
    }
}
