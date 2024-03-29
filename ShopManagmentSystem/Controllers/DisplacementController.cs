﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopManagmentSystem.BackgroundService;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Hubs;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.Service;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Controllers
{
    [Authorize]
    public class DisplacementController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDisplacementService _displacementService;
        private readonly IHubContext<UpdateHub> _updateHub;



        public DisplacementController(AppDbContext context, UserManager<AppUser> userManager,
            IDisplacementService displacementService, IHubContext<UpdateHub> updateHub)
        {
            _context = context;
            _userManager = userManager;
            _displacementService = displacementService;
            _updateHub = updateHub;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Displacement
                .Include(d => d.DisplacementProducts).ThenInclude(dp => dp.Product)
                .ToListAsync());
        }

        public async Task<IActionResult> Create(string search)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            ViewBag.Destination = new SelectList(await _context.Branches
                .Where(b => b.Id != user.BranchId && b.Id != 5).ToListAsync(), "Id", "Name");
            DisplacementCreateVM createVM;
            string basket = Request.Cookies["Basket"];
            if (basket == null)
            {
                createVM = new();
                createVM.Products = await _context.Products
                    .Include(p => p.Color)
                        .Include(p => p.ProductModel)
                        .Include(p => p.ProductCategory)
                        .Include(p => p.Brand)
                        .Where(p => !p.IsSold && p.BranchId == user.BranchId)
                        .ToListAsync();

            }
            else
            {
                createVM = new();
                List<int> itemsId = JsonConvert.DeserializeObject<List<ResendProductVM>>(basket).Select(i => i.Id).ToList();


                createVM.Products = await _context.Products
                    .Include(p => p.Color)
                    .Include(p => p.ProductModel)
                    .Include(p => p.ProductCategory)
                    .Include(p => p.Brand)
                    .Where(p => !itemsId.Contains(p.Id) && p.BranchId == user.BranchId && !p.IsSold).ToListAsync();

                ViewBag.List = JsonConvert.DeserializeObject<List<ResendProductVM>>(basket);
            }
            return View(createVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(DisplacementCreateVM createVM)
        {

            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            ViewBag.Destination = new SelectList(await _context.Branches
                .Where(b => b.Id != user.BranchId).ToListAsync(), "Id", "Name");
            string basket = Request.Cookies["basket"];
            if (string.IsNullOrWhiteSpace(basket))
            {
                createVM.Products = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Color)
                    .Include(p => p.ProductModel)
                    .Include(p => p.ProductCategory)
                    .Where(p => p.BranchId == user.BranchId && !p.IsSold)
                    .ToListAsync();
                ModelState.AddModelError("DestinationId", "Mehsul elave edilmeyib !");
                return View(createVM);
            }
            List<int> itemsId = JsonConvert.DeserializeObject<List<ResendProductVM>>(basket).Select(i => i.Id).ToList();
            List<Product> products = await _context.Products.Where(p => itemsId.Contains(p.Id)).ToListAsync();
            if (products.Count == 0 || products == null) return BadRequest();
            List<DisplacementProduct> displacementProducts = new();
            foreach (Product product in products)
            {
                product.BranchId = 5;
                DisplacementProduct displacementProduct = new();
                displacementProduct.ProductId = product.Id;
                displacementProducts.Add(displacementProduct);

            }
            Displacement displacement = new()
            {
                IsAcceppted = false,
                SenderId = user.BranchId,
                SenderBranch = _context.Branches.FirstOrDefault(b => b.Id == user.BranchId).Name,
                DestinationId = createVM.DestinationId,
                DestinationBranch = _context.Branches.FirstOrDefault(b => b.Id == createVM.DestinationId).Name,
                DisplacementProducts = displacementProducts,
                CreateDate = DateTime.Now,
                DeleteInfo = "Empty",
            };

            _displacementService.ScheduleDisplacement(displacement);

            await _context.Displacement.AddAsync(displacement);
            await _context.SaveChangesAsync();
            Response.Cookies.Delete("Basket");


            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> AddToList(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Product? product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductModel)
                .Include(p => p.Color)
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            string basket = Request.Cookies["Basket"];
            List<ResendProductVM> products;
            ResendProductVM resendProduct = new();
            resendProduct.Id = product.Id;
            resendProduct.ProductBrand = product.Brand.BrandName;
            resendProduct.Model = product.ProductModel.ModelName;
            resendProduct.Color = product.Color.ColorName;
            resendProduct.Series = product.Series;
            resendProduct.Category = product.ProductCategory.Name;

            if (basket == null)
            {
                products = new();
                products.Add(resendProduct);
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<ResendProductVM>>(basket);
                products.Add(resendProduct);
            }

            Response.Cookies.Append("Basket", JsonConvert.SerializeObject(products),
               new CookieOptions { MaxAge = TimeSpan.FromMinutes(5) });
            return Ok(resendProduct);
        }
        [HttpDelete]
        public IActionResult DeleteList(int? id)
        {
            if (id == null || id == 0) return BadRequest();
            string basket = Request.Cookies["Basket"];
            if (basket == null) return NotFound();
            List<ResendProductVM> products = JsonConvert.DeserializeObject<List<ResendProductVM>>(basket);
            ResendProductVM productVM = products.FirstOrDefault(p => p.Id == id);
            if (productVM == null) return BadRequest();
            products.Remove(productVM);
            Response.Cookies.Append("Basket", JsonConvert.SerializeObject(products),
               new CookieOptions { MaxAge = TimeSpan.FromMinutes(5) });

            return Ok(productVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (id == 0 || id == null || user == null) return BadRequest();
            Displacement? displacement = await _context.Displacement
                .Include(d => d.DisplacementProducts).ThenInclude(dp => dp.Product).ThenInclude(p => p.Brand)
                .Include(d => d.DisplacementProducts).ThenInclude(dp => dp.Product).ThenInclude(p => p.Color)
                .Include(d => d.DisplacementProducts).ThenInclude(dp => dp.Product).ThenInclude(p => p.ProductModel)
                .Include(d => d.DisplacementProducts).ThenInclude(dp => dp.Product).ThenInclude(p => p.ProductCategory)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (displacement == null) return NotFound();
            DisplacementDetailsVM detailsVM = new()
            {
                Displacement = displacement,
                User = user
            };
            return View(detailsVM);
        }

        public async Task<IActionResult> AcceptDisplacement(int? id)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return BadRequest();
            if (id == null || id == 0) return BadRequest();
            Displacement? displacement = await _context.Displacement
                .Include(d => d.DisplacementProducts).ThenInclude(dp => dp.Product)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (displacement == null) return NotFound();
            if (displacement.DestinationId != user.BranchId) return BadRequest();
            if (displacement.IsDeleted) return NotFound();
            foreach (var item in displacement.DisplacementProducts)
            {
                item.Product.BranchId = user.BranchId;
            }

            displacement.IsAcceppted = true;
            await _context.SaveChangesAsync();
            TempData["ok"] = true;
            await _updateHub.Clients.All.SendAsync("AcceptDisplacement", displacement.Id);


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return BadRequest();
            Displacement? displacement = await _context.Displacement
                .Include(d => d.DisplacementProducts)
                .ThenInclude(dp => dp.Product)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (displacement == null || displacement.IsAcceppted) return NotFound();

            foreach (var item in displacement.DisplacementProducts)
            {
                item.Product.BranchId = displacement.SenderId;
            }

            displacement.IsDeleted = true;
            displacement.DeleteInfo = $"Deleted {displacement.SenderBranch}";
            await _context.SaveChangesAsync();
            await _updateHub.Clients.All.SendAsync("DeleteDisplacement", id);
            return Ok();
        }
    }
}
