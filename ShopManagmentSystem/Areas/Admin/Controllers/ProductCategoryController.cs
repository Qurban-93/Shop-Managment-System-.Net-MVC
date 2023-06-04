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
            return View(_context.ProductCategories.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategoryCreateVM createVM)
        {
            if(!ModelState.IsValid) return View(createVM);
            if(_context.ProductCategories.Any(pc=>pc.Name == createVM.Name)) 
            {
                ModelState.AddModelError("Name","Bu adli categoriya movcuddur !");
                return View();
            }
            ProductCategory productCategory = new();
            productCategory.Name = createVM.Name;
            productCategory.Bonus = createVM.Bonus;
            productCategory.CreateDate = DateTime.Now;
            _context.ProductCategories.Add(productCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            ProductCategory? productCategory = await _context.ProductCategories.FirstOrDefaultAsync(pc=>pc.Id == id);
            if (productCategory == null) return NotFound();
            _context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync();
            return Ok(productCategory.Name);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            ProductCategory? productCategory = await _context.ProductCategories.FirstOrDefaultAsync(pc=>pc.Id==id);
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
        public async Task<IActionResult> Edit(int? id,ProductCategoryEditVM editVM)
        {
            if(id == null || id == 0) return NotFound();
            if(!ModelState.IsValid) return View(editVM);
            ProductCategory? existProdCategory = await _context.ProductCategories.FirstOrDefaultAsync(pc=>pc.Id==id);
            if (existProdCategory == null) return NotFound();
            if(_context.ProductCategories.Any(pc=>pc.Id != id && pc.Name == editVM.Name))
            {
                ModelState.AddModelError("Name", "Bu adli Product Categoriya var !");
                return View(editVM);
            }
            existProdCategory.Name = editVM.Name;
            existProdCategory.Bonus = editVM.Bonus;
            existProdCategory.UpdateDate = DateTime.Now;
            await _context.SaveChangesAsync();
            TempData["Edit"] = true;

            return RedirectToAction(nameof(Index));
        }
    }
}
