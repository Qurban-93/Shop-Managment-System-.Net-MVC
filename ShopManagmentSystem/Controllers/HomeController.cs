﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;
using System.Diagnostics;

namespace ShopManagmentSystem.Controllers
{
    public class HomeController : Controller
    {    
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? search)
        {   
            if(!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if(user == null) return NotFound();
            HomeVM home = new HomeVM();
            List<Product> products = new();
            var query = _appDbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Brand)
                .Include(p => p.Color)
                .Where(p => !p.IsSold && p.BranchId == user.BranchId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                products = await query.Where(p => 
                p.Name.Contains(search.Trim().ToLower()) ||
                p.Brand.BrandName.Contains(search.Trim().ToLower()) ||
                p.ProductCategory.Name.Contains(search.Trim().ToLower())).ToListAsync();
              
            }
            else
            {
                products = await query.ToListAsync();
            }

            List<Order> orders =await _appDbContext.Orders.Where(o=>o.BranchId == user.BranchId)
                .ToListAsync();        
                
            if (orders != null || orders.Count > 0 )
            {       
                foreach (var item in orders)
                {
                    if (products.Any(p => p.Id == item.ProdId))
                    {
                        products.Remove(products.FirstOrDefault(p => p.Id == item.ProdId));
                    }
                }

            }
            
            home.Products = products;
            ViewBag.SearchValue = search;

            return View(home);
        }
     
    }
}