﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;

        public SaleController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string search,DateTime? fromDate,DateTime? toDate)
        {
            if (!User.Identity.IsAuthenticated) return NotFound();
            
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(search))
            {
                if (fromDate < toDate)
                {
                    ViewBag.Error = "Invalid data";
                    return View();
                }
                if (fromDate != null && toDate == null)
                {
                    ViewBag.FromDate = fromDate;
                    return View(await _context.Sales
                .Include(s => s.Products)
                .Include(s => s.Customer)
                .Include(s => s.Employee)
                .Include(s => s.Branch)
                .Where(s => s.Customer.FullName.Contains(search.Trim().ToLower()) && s.CreateDate>fromDate)
                .Where(s => s.BranchId == user.BranchId)
                .ToListAsync());
                }
                if (fromDate == null && toDate != null)
                {
                    ViewBag.ToDate = toDate;
                    return View(await _context.Sales
                .Include(s => s.Products)
                .Include(s => s.Customer)
                .Include(s => s.Employee)
                .Include(s => s.Branch)
                .Where(s => s.Customer.FullName.Contains(search.Trim().ToLower()) && s.CreateDate < toDate)
                .Where(s => s.BranchId == user.BranchId)
                .ToListAsync());
                }
                if(fromDate != null && toDate != null)
                {
                    ViewBag.FromDate = fromDate;
                    ViewBag.ToDate = toDate;
                    return View(await _context.Sales
                .Include(s => s.Products)
                .Include(s => s.Customer)
                .Include(s => s.Employee)
                .Where(s => s.Customer.FullName.Contains(search.Trim().ToLower()) 
                && s.CreateDate > fromDate && s.CreateDate < toDate)
                .Where(s => s.BranchId == user.BranchId)
                .ToListAsync());
                }
                return View(await _context.Sales
                .Include(s => s.Products)
                .Include(s => s.Customer)
                .Include(s => s.Employee)
                .Include(s => s.Branch)
                .Include(s=>s.Branch)
                .Where(s=>s.Customer.FullName.Contains(search.Trim().ToLower()))
                .Where(s=>s.BranchId == user.BranchId)
                .ToListAsync());
            }
            if (fromDate > toDate)
            {   
                ViewBag.Error = "Invalid data";

                return View(await _context.Sales
               .Include(s => s.Products)
               .Include(s => s.Customer)
               .Include(s => s.Employee)
               .Where(s => s.CreateDate > DateTime.Today)
               .ToListAsync());
            }
            if (fromDate != null && toDate == null)
            {
                ViewBag.FromDate = fromDate;
                return View(await _context.Sales
            .Include(s => s.Products)
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.Branch)
            .Where(s =>s.CreateDate > fromDate)
            .Where(s => s.BranchId == user.BranchId)
            .ToListAsync());
            }
            if (fromDate == null && toDate != null)
            {
                ViewBag.ToDate = toDate;
                
                return View(await _context.Sales
            .Include(s => s.Products)
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.Branch)
            .Where(s => s.CreateDate < toDate)
            .Where(s => s.BranchId == user.BranchId)
            .ToListAsync());
            }
            if (fromDate != null && toDate != null)
            {
                ViewBag.FromDate = fromDate.ToString();
                ViewBag.ToDate = toDate.ToString();
                return View(await _context.Sales
            .Include(s => s.Products)
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.Branch)
            .Where(s => s.CreateDate > fromDate && s.CreateDate < toDate)
            .Where(s => s.BranchId == user.BranchId)
            .ToListAsync());
            }

            return View(await _context.Sales
                .Include(s=>s.Products)
                .Include(s=>s.Customer)
                .Include(s=>s.Employee)
                .Include(s => s.Branch)
                .Where(s=>s.CreateDate > DateTime.Today)
                .Where(s => s.BranchId == user.BranchId)
                .ToListAsync());
        }




        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated) return BadRequest();
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();

            if (id == null || id == 0) return NotFound();
            List<Order> orders = _context.Orders.Where(o=>o.BranchId == user.BranchId)
                .ToList();

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
            if (!User.Identity.IsAuthenticated) return BadRequest();
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            if (id == null || id == 0) return NotFound();

            Product product = await _context.Products
                .Include(p => p.Color)
                .Include(p => p.Brand)
                .Include(p=>p.ProductCategory)
                .Where(p=>p.BranchId == user.BranchId)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            List<Order> orders = await _context.Orders.Where(o=>o.BranchId == user.BranchId)
                .ToListAsync();

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
            order.BranchId = user.BranchId;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
       
            return Ok();
        }


        public async Task<IActionResult> Orders()
        {
            if (!User.Identity.IsAuthenticated) return BadRequest();
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            List<Order> orders = await _context.Orders.Where(o=>o.BranchId==user.BranchId).ToListAsync();
            SaleVM saleVM = new SaleVM();
            saleVM.Orders = orders;         
            ViewBag.Sellers = new SelectList( await _context.Employees
                .Where(e=>e.BranchId == user.BranchId).ToListAsync(),"Id", "FullName");
            return View(saleVM);
        }

        [HttpPost]
        public async Task<IActionResult> Orders(SaleVM saleVM)
         {
            if (!User.Identity.IsAuthenticated) return BadRequest();
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            List<Order> orders = _context.Orders.ToList();      
            ViewBag.Sellers = new SelectList(await _context.Employees
                .Where(e=>e.BranchId == user.BranchId).ToListAsync(), "Id", "FullName");
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
            newSale.CreateDate = DateTime.Now;
            newSale.BranchId = user.BranchId;
            customer.TotalCost += products.Sum(p => p.Price) - saleVM.Discount;


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
