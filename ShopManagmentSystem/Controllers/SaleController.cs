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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            List<Order> orders = _context.Orders.ToList();

            if (orders.Any(p => p.ProdId == id))
            {
                double price = orders.FirstOrDefault(p => p.ProdId == id).Price;
                _context.Orders.Remove(orders.FirstOrDefault(p => p.ProdId == id));
                await _context.SaveChangesAsync();
                
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
            
            List<Order> orders = await _context.Orders.ToListAsync();

            if (orders.Any(o=>o.ProdId == product.Id))
            {
                return NotFound();
            }
                 
            Order order = new();
            order.Name = product.Name;
            order.Series = product.Series;
            order.Price = product.Price;
            order.Category = product.ProductCategory.Name;
            order.Color = product.Color.ColorName;
            order.Brand = product.Brand.BrandName;
            order.ProdId = product.Id;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
       
            return Ok();
        }


        public async Task<IActionResult> Orders()
        {
            List<Order> orders = _context.Orders.ToList();
            SaleVM saleVM = new SaleVM();
            saleVM.Orders = orders;         
            ViewBag.Sellers = new SelectList(_context.Employees.ToList(),"Id", "FullName");
            return View(saleVM);
        }

        [HttpPost]
        public async Task<IActionResult> Orders(SaleVM saleVM)
         {
            List<Order> orders = _context.Orders.ToList();      
            ViewBag.Sellers = new SelectList(_context.Employees.ToList(), "Id", "FullName");
            if (!ModelState.IsValid)
            {
                saleVM.Orders = orders;           
                return View(saleVM);
            }
            if(orders.Sum(o => o.Price) < saleVM.CashlessPayment)
            {
                ModelState.AddModelError("CashlessPayment", "Nagdsiz odenis umumi meblegden cox ola bilmez !");
                saleVM.Orders = orders;            
                return View(saleVM);
            }
            if(saleVM.CashlessPayment < 0)
            {
                ModelState.AddModelError("CashlessPayment", "Nagdsiz odenis menfi ola bilmez !");
                saleVM.Orders = orders;
                return View(saleVM);
            }
            if(saleVM.Discount< 0)
            {
                ModelState.AddModelError("Discount", "Endirim menfi ola bilmez !");
                saleVM.Orders = orders;
                return View(saleVM);
            }
            if(saleVM.Discount > 500)
            {
                ModelState.AddModelError("Discount", "Endirim limitini kecmisiz !");
                saleVM.Orders = orders;
                return View(saleVM);
            }
            Customer? customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.PhoneNumber == saleVM.CustomerEmailOrPhoneNumber);
            if (customer == null)
            {
                customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == saleVM.CustomerEmailOrPhoneNumber);
            }
            if(customer == null)
            {
                ModelState.AddModelError("CustomerEmailOrPhoneNumber", "Alici bazada tapilmadi !");
                saleVM.Orders = orders;
                return View(saleVM);
            }
            
            List<int> prodId = new List<int>();
            foreach (Order order in orders)
            {
                prodId.Add(order.ProdId);
            }
            List<Product> products = _context.Products.Where(p => prodId.Contains(p.Id)).ToList();

            Sale newSale = new();
            newSale.CustomerId = customer.Id;
            newSale.Products = products;
            newSale.TotalPrice = products.Sum(p => p.Price) - saleVM.Discount;
            newSale.Discount = saleVM.Discount;
            newSale.CashlessPayment = saleVM.CashlessPayment;
            newSale.TotalProfit = (products.Sum(p => p.Price) - saleVM.Discount) - products.Sum(p => p.CostPrice);
            newSale.EmployeeId = saleVM.EmployeeId;

            foreach (var item in products)
            {
                item.IsSold = true;
            }
            _context.Sales.Add(newSale);
            _context.Orders.RemoveRange(orders);         
            await _context.SaveChangesAsync();
            TempData["Success"] = "ok";

            return RedirectToAction("index","home");
        }
    }
}
