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

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {   
            HomeVM home = new HomeVM();


            List<Order> orders =await _appDbContext.Orders.ToListAsync();         
            List<Product> products = await _appDbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p=>p.Brand)
                .Include(p => p.Color)
                .Where(p=>!p.IsSold)
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

            return View(home);
        }
     
    }
}