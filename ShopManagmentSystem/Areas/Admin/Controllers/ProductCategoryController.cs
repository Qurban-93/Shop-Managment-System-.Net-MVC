using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels.ProductCategoryVMs;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class ProductCategoryController : Controller
    {
        private readonly AppDbContext _context;
        public ProductCategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.ProductCategories.Where(pc => !pc.IsDeleted).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategoryCreateVM createVM)
        {
            if (!ModelState.IsValid) return View(createVM);
            if (_context.ProductCategories.Any(pc => pc.Name == createVM.Name))
            {
                ModelState.AddModelError("Name", "Bu adli categoriya movcuddur !");
                return View();
            }
            if (createVM.SeriesUniqueRequired && createVM.SeriesMaxMinLength == null)
            {
                ModelState.AddModelError("SeriesMaxMinLength", "Bosh olmaz !");
                return View();
            }
            ProductCategory productCategory = new();
            productCategory.Name = createVM.Name;
            productCategory.Bonus = createVM.Bonus;
            productCategory.CreateDate = DateTime.Now;
            productCategory.SeriesMaxLength = createVM.SeriesMaxMinLength;
            productCategory.SeriesUniqueRequired = createVM.SeriesUniqueRequired;
            _context.ProductCategories.Add(productCategory);
            await _context.SaveChangesAsync();
            TempData["Create"] = true;
            return RedirectToAction(nameof(Index));
        }
       
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (id == null || id == 0) return NotFound();
            ProductCategory? productCategory = await _context.ProductCategories.FirstOrDefaultAsync(pc => pc.Id == id && !pc.IsDeleted);
            if (productCategory == null) return NotFound();
            ProductCategoryEditVM editVM = new()
            {
                Name = productCategory.Name,
                Bonus = productCategory.Bonus,
            };
            return View(editVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ProductCategoryEditVM editVM)
        {
            if (id == null || id == 0) return NotFound();
            if (!ModelState.IsValid) return View(editVM);
            ProductCategory? existProdCategory = await _context.ProductCategories.FirstOrDefaultAsync(pc => pc.Id == id && !pc.IsDeleted);
            if (existProdCategory == null) return NotFound();
            if (_context.ProductCategories.Any(pc => pc.Id != id && pc.Name == editVM.Name))
            {
                ModelState.AddModelError("Name", "Bu adli Product Categoriya var !");
                return View(editVM);
            }
            if(editVM.SeriesUniqueRequired && editVM.SeriesMaxMinLength == null)
            {
                ModelState.AddModelError("SeriesMaxMinLength", "Bosh olmaz !");
                return View(editVM);
            }
            existProdCategory.Name = editVM.Name;
            existProdCategory.Bonus = editVM.Bonus;
            existProdCategory.UpdateDate = DateTime.Now;
            existProdCategory.SeriesMaxLength = editVM.SeriesMaxMinLength;
            existProdCategory.SeriesUniqueRequired = editVM.SeriesUniqueRequired;
            await _context.SaveChangesAsync();
            TempData["Edit"] = true;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            ProductCategory? productCategory = await _context.ProductCategories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (productCategory == null) return NotFound();
            productCategory.IsDeleted= true;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
