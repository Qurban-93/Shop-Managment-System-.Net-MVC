using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;
using System.Diagnostics;

namespace ShopManagmentSystem.Controllers
{
    public class HomeController : Controller
    {    
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index(string? search)
        {
            HomeVM home = new HomeVM();
            var query = _appDbContext.Products
                    .Include(p => p.ProductCategory)
                    .Include(p=>p.Brand)
                    .Include(p => p.Color);    
            
            if (!string.IsNullOrWhiteSpace(search))
            {
               var searchProd =  query.Where(p=>p.Name.Contains(search) || p.Brand.BrandName.Contains(search)).ToList();        
                home.Products = searchProd;
                return View(home);
            }
            
            var products = query.ToList();
            home.Products = products;

            return View(home);
        }
     
    }
}