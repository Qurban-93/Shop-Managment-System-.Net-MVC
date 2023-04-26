using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Controllers
{
    public class SaleController : Controller
    {
        private readonly AppDbContext _context;

        public SaleController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Product product = await _context.Products
                .Include(p => p.Color)
                .Include(p => p.Brand)
                .Include(p=>p.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            List<ProductVM> products;
            string basket = Request.Cookies["basket"];
            if (basket == null)
            {
                products = new();
                
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<ProductVM>>(basket);            
            }
            var existProd = products.FirstOrDefault(p => p.Id == id);
            if (existProd != null)
            {
                return BadRequest();
            }
            ProductVM productVM = new();
            productVM.Name = product.Name;
            productVM.Series = product.Series;
            productVM.Price = product.Price;
            productVM.Category = product.ProductCategory.Name;
            productVM.Color = product.Color.ColorName;
            productVM.Brand = product.Brand.BrandName;
            productVM.Id = product.Id;

            products.Add(productVM);
            Response.Cookies.Append("Basket", JsonConvert.SerializeObject(products),
            new CookieOptions { MaxAge = TimeSpan.FromMinutes(1) });

            return Ok();
        }


        public async Task<IActionResult> Orders()
        {
            List<ProductVM> products = new();
            string basket = Request.Cookies["basket"];
            if (basket != null)
            {
                products = JsonConvert.DeserializeObject<List<ProductVM>>(basket);
            }      
            return View(products);
        }
    }
}
