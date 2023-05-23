﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels.ProductModelVMs;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductModelController : Controller
    {
        private readonly AppDbContext _context;

        public ProductModelController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.ProductModels
                .Include(p => p.ProductCategory)
                .Include(p => p.Brand).ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "BrandName");
            ViewBag.ProductCategories = new SelectList(_context.ProductCategories.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModelCreateVM createVM)
        {

            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "BrandName");
            ViewBag.ProductCategories = new SelectList(_context.ProductCategories.ToList(), "Id", "Name");
            if (!ModelState.IsValid) return View();
            if (await _context.Brands.FirstOrDefaultAsync(b => b.Id == createVM.BrandId) == null)
            {
                ModelState.AddModelError("BrandId", "Error Brand option !");
                return View();
            }
            if (await _context.ProductCategories.FirstOrDefaultAsync(pc => pc.Id == createVM.ProductCategoryId) == null)
            {
                ModelState.AddModelError("ProductCategoryId", "Category options error !");
                return View();
            }
            if(await _context.ProductModels.AnyAsync(pm => pm.ModelName.Trim().ToLower() == createVM.ModelName.Trim().ToLower() && 
            pm.BrandId == createVM.BrandId))
            {
                ModelState.AddModelError("ModelName", "Bu adla model movcuddur !");
                return View();
            }
            ProductModel productModel = new()
            {
                BrandId = createVM.BrandId,
                ProductCategoryId = createVM.ProductCategoryId,
                ModelPrice = createVM.ModelPrice,
                ModelName = createVM.ModelName.Trim().ToUpper(),
                CreateDate = DateTime.Now,
            };
            _context.ProductModels.Add(productModel);
            await _context.SaveChangesAsync();
            TempData["Create"] = true;


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "BrandName");
            ViewBag.ProductCategories = new SelectList(_context.ProductCategories.ToList(), "Id", "Name");
            if (id == null || id == 0) return View();
            ProductModel? productModel = await _context.ProductModels.FirstOrDefaultAsync(p => p.Id == id);
            if (productModel == null) return View();
            ProductModelEditVM editVM = new()
            {
                ModelName = productModel.ModelName,
                ModelPrice = productModel.ModelPrice,
                ProductCategoryId = (int)productModel.ProductCategoryId,
                BrandId = (int)productModel.BrandId
            };
            return View(editVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductModelEditVM editVM, int? id)
        {
            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "BrandName");
            ViewBag.ProductCategories = new SelectList(_context.ProductCategories.ToList(), "Id", "Name");
            if (!ModelState.IsValid) return View();
            if (await _context.Brands.FirstOrDefaultAsync(b => b.Id == editVM.BrandId) == null)
            {
                ModelState.AddModelError("BrandId", "Error Brand option !");
                return View();
            }
            if (await _context.ProductCategories.FirstOrDefaultAsync(pc => pc.Id == editVM.ProductCategoryId) == null)
            {
                ModelState.AddModelError("ProductCategoryId", "Category options error !");
                return View();
            }
            if (_context.ProductModels.Any(pm => pm.ModelName.Trim().ToLower() == editVM.ModelName.Trim().ToLower() && pm.Id != id))
            {
                ModelState.AddModelError("ModelName", "Bu adla model movcuddur !");
                return View();
            }
            if (id == null || id == 0) return NotFound();
            ProductModel? productModel = await _context.ProductModels.FirstOrDefaultAsync(pm => pm.Id == id);
            if (productModel == null) return NotFound();
            productModel.UpdateDate = DateTime.Now;
            productModel.ModelPrice = editVM.ModelPrice;
            productModel.ModelName = editVM.ModelName;
            productModel.BrandId = editVM.BrandId;
            productModel.ProductCategoryId = editVM.ProductCategoryId;

            await _context.SaveChangesAsync();
            TempData["Edit"] = true;

            return RedirectToAction("Index");
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == 0 || id == null) return NotFound();
            ProductModel? productModel = await _context.ProductModels.FirstOrDefaultAsync(p=>p.Id== id);
            if (productModel == null) return NotFound();
            _context.ProductModels.Remove(productModel);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}