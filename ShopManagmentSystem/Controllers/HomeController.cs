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

        public IActionResult Index()
        {   
            HomeVM home = new HomeVM();

            var products = _appDbContext.Products
                    .Include(p => p.ProductCategory)
                    .Include(p=>p.Brand)
                    .Include(p => p.Color).ToList();

            List<ProductVM> productsInBasket;

            string basket = Request.Cookies["basket"];

            if (basket == null)
            {
                productsInBasket = new();

            }
            else
            {
                productsInBasket = JsonConvert.DeserializeObject<List<ProductVM>>(basket);
                foreach (var item in productsInBasket)
                {
                    if (products.Any(p=>p.Id == item.Id))
                    {
                        products.Remove(products.FirstOrDefault(p => p.Id == item.Id));
                    }
                }
            }

            //if (!string.IsNullOrWhiteSpace(search))
            //{
            //   var searchProd =  query.Where(p=>p.Name.Contains(search) || p.Brand.BrandName.Contains(search)).ToList();
            //    foreach (var item in searchProd)
            //    {
            //        if(productsBasket.Any(p=>p.Id == item.Id))
            //        {
            //            searchProd.Remove(item);
            //        }
            //    }

            //    home.Products = searchProd;
            //    return View(home);
            //}

            home.Products = products;

            return View(home);
        }
     
    }
}