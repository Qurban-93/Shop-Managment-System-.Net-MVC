using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels.BrandVMs;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;


        public BrandController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Brands.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BrandCreateVM brandCreateVM)
        {
            if (!ModelState.IsValid) return View();
            if(_context.Brands.Any(b=>b.BrandName == brandCreateVM.BrandName))
            {
                ModelState.AddModelError("BrandName", "Bu adli Brand artiq movcuddur!");
                return View();
            }
            Brand brand = new();
            brand.BrandName= brandCreateVM.BrandName;
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            TempData["Success"] = "ok";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null || id == 0) return NotFound();
            Brand? brand = await _context.Brands.FirstOrDefaultAsync(b =>b.Id == id);
            if(brand == null) return NotFound();
            BrandEditVM editVM = new()
            {
                BrandName = brand.BrandName,
            };
            

            return View(editVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(BrandEditVM editVM,int? id)
        {

            if(!ModelState.IsValid) return View();
            if(id == null || id == 0) { return View(); }
            Brand brand = await _context.Brands.FirstOrDefaultAsync(b=>b.Id == id);
            if(brand == null) return NotFound();
            if(_context.Brands.Any(b=>b.BrandName == editVM.BrandName && b.Id != id))
            {
                ModelState.AddModelError("BrandName", "Bu adla Brand Movcuddur !");
                return View();
            }
            brand.BrandName = editVM.BrandName;
            await _context.SaveChangesAsync();
            TempData["Edit"] = "ok";
            return RedirectToAction(nameof(Index));
            
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || id == 0) return NotFound();
            Brand? brand =await _context.Brands.FirstOrDefaultAsync(b=>b.Id == id);
            if(brand == null) return NotFound();
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return Ok();

        }

    }
}
