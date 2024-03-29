﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels.ProductVMs;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
                .Where(p=>p.BranchId == user.BranchId && !p.IsDeleted).ToListAsync();
                
            return View(products);
        } 
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Brands = new SelectList(_context.Brands.Where(b=>!b.IsDeleted).ToList(), "Id", "BrandName");         
            ViewBag.Color = new SelectList(_context.Colors.Where(c => !c.IsDeleted).ToList(), "Id", "ColorName");
            ViewBag.ProdModel = new SelectList(_context.ProductModels.Where(pm => !pm.IsDeleted && pm.BrandId == _context.Brands.First().Id).ToList(), "Id", "ModelName");
            ProductModel productModel = _context.ProductModels.Where(pm => !pm.IsDeleted && pm.BrandId == _context.Brands.First().Id).Include(pm=>pm.ProductCategory).First();         
           
            ViewBag.Disabled = productModel.ProductCategory.SeriesUniqueRequired;          
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.Brands = new SelectList(_context.Brands.Where(b => !b.IsDeleted).ToList(), "Id", "BrandName");
            ViewBag.Color = new SelectList(_context.Colors.Where(c => !c.IsDeleted).ToList(), "Id", "ColorName");
            ViewBag.ProdModel = new SelectList(_context.ProductModels.Where(pm => pm.BrandId == productCreateVM.BrandId && !pm.IsDeleted).ToList(), "Id", "ModelName");
            ViewBag.Disabled = false;
            
            Brand? brand = await _context.Brands.Include(b => b.ProductModels).FirstOrDefaultAsync(b => b.Id == productCreateVM.BrandId);
            ProductModel? productModel = await _context.ProductModels.Include(pm=>pm.ProductCategory).FirstOrDefaultAsync(pm => pm.Id == productCreateVM.ProductModelId);
            Color? color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == productCreateVM.ColorId);
            if (productModel == null || brand == null || color == null) return NotFound();

            ViewBag.Disabled = productModel.ProductCategory.SeriesUniqueRequired;


            if (!ModelState.IsValid) return View();

            Product product = new();
       
            
            ProductCategory? productCategory = await _context.ProductCategories.FirstOrDefaultAsync(pc => pc.Id == productModel.ProductCategoryId);
            if(productCategory == null) return NotFound();
            if (productCategory.SeriesUniqueRequired)
            {
                if(productCreateVM.Series == null)
                {
                    ModelState.AddModelError("Series", "Bosh olmaz !");
                    return View();
                }
                if(productCreateVM.Series.Length < productCategory.SeriesMaxLength || productCreateVM.Series.Length > productCategory.SeriesMaxLength)
                {
                    ModelState.AddModelError("Series", $"Imei seriya {productCategory.SeriesMaxLength} reqemden az ve ya cox ola bilmez!");
                    return View();
                }
                if(await _context.Products.Where(p=>p.ProductCategoryId == productCategory.Id).AnyAsync(p=>p.Series == productCreateVM.Series))
                {
                    ModelState.AddModelError("Series", "Imei artiq movcuddur!");
                    return View();
                }
            }
            else
            {
                product.Series = "";
            } 
            
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

            
            product.ProductModelId = productCreateVM.ProductModelId;
            product.ProductCategoryId = (int)productModel.ProductCategoryId;
            product.ColorId = productCreateVM.ColorId;
            product.BrandId = productCreateVM.BrandId;
            product.BranchId = user.BranchId;
            product.CostPrice = productCreateVM.CostPrice;
            product.CreateDate = DateTime.Now;
            
            product.IsSold = false;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            TempData["Create"] = true;

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Brands = new SelectList(_context.Brands.Where(b => !b.IsDeleted).ToList(), "Id", "BrandName");
            ViewBag.Color = new SelectList(_context.Colors.Where(c => !c.IsDeleted).ToList(), "Id", "ColorName");

            if (id == null || id == 0) return NotFound();
            Product? product = await _context.Products.Include(p=>p.ProductCategory).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return NotFound();
            ViewBag.ProdModel = new SelectList(_context.ProductModels.Where(pm=>pm.BrandId == product.BrandId && !pm.IsDeleted ).ToList(), "Id", "ModelName");
            ProductEditVM editVM = new()
            {
                CostPrice = product.CostPrice,
                BrandId= product.BrandId,
                ColorId = product.ColorId,              
                ProductModelId = product.ProductModelId,
                Series = product.Series,
            };

            ViewBag.Disabled = product.ProductCategory.SeriesUniqueRequired;

            return View(editVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id ,ProductEditVM editVM)
        {
            ViewBag.Brands = new SelectList(_context.Brands.Where(b => !b.IsDeleted).ToList(), "Id", "BrandName");
            ViewBag.Color = new SelectList(_context.Colors.Where(c => !c.IsDeleted).ToList(), "Id", "ColorName");
            Product? product = await _context.Products.Include(p => p.ProductCategory).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return NotFound();
            ViewBag.ProdModel = new SelectList(_context.ProductModels.Where(pm => pm.BrandId == product.BrandId && !pm.IsDeleted).ToList(), "Id", "ModelName");
            ViewBag.Disabled = product.ProductCategory.SeriesUniqueRequired;
            if (!ModelState.IsValid) return View();
            if(id == 0 || id == null ) return NotFound();
           
           
            Brand? brand = await _context.Brands.Include(b => b.ProductModels).FirstOrDefaultAsync(b => b.Id == editVM.BrandId);
            ProductModel? productModel = await _context.ProductModels.FirstOrDefaultAsync(pm => pm.Id == editVM.ProductModelId);
            Color? color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == editVM.ColorId);           
            if (productModel == null || brand == null || color == null) return NotFound();
            ProductCategory? productCategory = await _context.ProductCategories.FirstOrDefaultAsync(pc => pc.Id == productModel.ProductCategoryId);
            if (productCategory == null) return NotFound();
            if (productCategory.SeriesUniqueRequired)
            {
                if (editVM.Series == null)
                {
                    ModelState.AddModelError("Series", "Bosh olmaz !");
                    return View();
                }
                if (editVM.Series.Trim().Length < productCategory.SeriesMaxLength || editVM.Series.Trim().Length > productCategory.SeriesMaxLength)
                {
                    ModelState.AddModelError("Series", $"Seriya {productCategory.SeriesMaxLength} reqemden az ve ya cox ola bilmez!");
                    return View();
                }
                if (await _context.Products.Where(p => p.ProductCategoryId == productCategory.Id).AnyAsync(p => p.Series == editVM.Series && p.Id != id))
                {
                    ModelState.AddModelError("Series", "Seriya artiq movcuddur!");
                    return View();
                }
                product.Series = editVM.Series.Trim();
            }
            else
            {
                product.Series = "";
            }
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
            product.UpdateDate = DateTime.Now;
            product.ProductCategoryId = (int)productModel.ProductCategoryId;       

            await _context.SaveChangesAsync();
            TempData["Edit"] = true;
            return RedirectToAction("Index");
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return BadRequest();
            Product? product = await _context.Products.Include(p=>p.SaleProducts).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return NotFound();
            if (product.SaleProducts.Count > 0) return BadRequest("Relation");

            product.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> GetModels(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Brand? brand = await _context.Brands.Include(b => b.ProductModels.Where(pm=>!pm.IsDeleted)).FirstOrDefaultAsync(x => x.Id == id);
            if (brand == null) return NotFound();
            var options = new SelectList(brand.ProductModels, "Id", "ModelName");
            return Ok(options);
        }

        public async Task<IActionResult> GetUniqueSeries(int? id)
        {
            if(id == null || id == 0) return BadRequest();
            ProductModel? productModel = await _context.ProductModels.Include(pm=>pm.ProductCategory).FirstOrDefaultAsync(x => x.Id == id);
            if (productModel == null) return NotFound();
            bool check = productModel.ProductCategory.SeriesUniqueRequired;
            return Ok(check);
        }

    }
}
