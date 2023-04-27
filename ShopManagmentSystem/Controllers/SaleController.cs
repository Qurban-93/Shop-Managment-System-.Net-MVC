using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            List<ProductVM> products;
            string basket = Request.Cookies["basket"];
            if (basket == null) return NotFound();       
            products = JsonConvert.DeserializeObject<List<ProductVM>>(basket);
            if (products.Any(p => p.Id == id))
            {
                double price = products.FirstOrDefault(p => p.Id == id).Price;
                products.Remove(products.FirstOrDefault(p => p.Id == id));

                Response.Cookies.Append("Basket", JsonConvert.SerializeObject(products),
                new CookieOptions { MaxAge = TimeSpan.FromMinutes(10) });
                return Ok(price);
            }
                       
            return NotFound();
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
            new CookieOptions { MaxAge = TimeSpan.FromMinutes(10) });

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
            SaleVM saleVM = new SaleVM();
            saleVM.Products = products;
            saleVM.TotalPrice = products.Sum(p => p.Price);
            ViewBag.Sellers = new SelectList(_context.Employees.ToList(),"Id", "FullName");
            return View(saleVM);
        }

        [HttpPost]
        public async Task<IActionResult> Orders(SaleVM saleVM)
        {
            if (!ModelState.IsValid)
            {
                List<ProductVM> products = new();
                string basket = Request.Cookies["basket"];
                if (basket != null)
                {
                    products = JsonConvert.DeserializeObject<List<ProductVM>>(basket);
                }                
                saleVM.Products = products;
                saleVM.TotalPrice = products.Sum(p => p.Price);
                ViewBag.Sellers = new SelectList(_context.Employees.ToList(), "Id", "FullName");
                return View(saleVM);
            }
            if (saleVM == null) return BadRequest();
            return View(saleVM);
        }
    }
}
