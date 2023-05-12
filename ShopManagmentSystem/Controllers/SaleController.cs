﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Controllers;

[Authorize]
public class SaleController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public SaleController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public async Task<IActionResult> Index(string search, DateTime? fromDate, DateTime? toDate)
    {
        if (!User.Identity.IsAuthenticated) return NotFound();

        AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

        if (user == null) return NotFound();

        ViewBag.toDate = toDate;
        ViewBag.fromDate = fromDate;

        if (!string.IsNullOrWhiteSpace(search))
        {
            ViewBag.SearchValue = search;
            if (fromDate < toDate)
            {
                ViewBag.Error = "Invalid data";
                return View(await _context.Sales
                .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
                .Include(s => s.Customer)
                .Include(s => s.Employee)
                .Include(s => s.Branch)
                .Include(s => s.Branch)
                .Where(s => s.Customer.FullName.Contains(search.Trim().ToLower()) &&
                s.BranchId == user.BranchId).ToListAsync());
            }
            if (fromDate != null && toDate == null)
            {
                ViewBag.FromDate = fromDate;
                return View(await _context.Sales
            .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.Branch)
            .Where(s => s.Customer.FullName.Contains(search.Trim().ToLower()) &&
            s.CreateDate > fromDate && s.BranchId == user.BranchId).ToListAsync());
            }
            if (fromDate == null && toDate != null)
            {
                ViewBag.ToDate = toDate;
                return View(await _context.Sales
            .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.Branch)
            .Where(s => s.Customer.FullName.Contains(search.Trim().ToLower()) &&
            s.CreateDate < toDate && s.BranchId == user.BranchId).ToListAsync());
            }
            if (fromDate != null && toDate != null)
            {
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                return View(await _context.Sales
            .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Where(s => s.Customer.FullName.Contains(search.Trim().ToLower())
            && s.CreateDate > fromDate && s.CreateDate < toDate && s.BranchId == user.BranchId)
            .ToListAsync());
            }
            return View(await _context.Sales
            .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.Branch)
            .Include(s => s.Branch)
            .Where(s => s.Customer.FullName.Contains(search.Trim().ToLower()) &&
            s.BranchId == user.BranchId).ToListAsync());
        }
        if (fromDate > toDate)
        {
            ViewBag.Error = "Invalid data";

            return View(await _context.Sales
           .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
           .Include(s => s.Customer)
           .Include(s => s.Employee)
           .Include(s => s.Branch)
           .Where(s => s.CreateDate > DateTime.Today && s.BranchId == user.BranchId)
           .ToListAsync());
        }
        if (fromDate != null && toDate == null)
        {

            return View(await _context.Sales
        .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
        .Include(s => s.Customer)
        .Include(s => s.Employee)
        .Include(s => s.Branch)
        .Where(s => s.CreateDate > fromDate)
        .Where(s => s.BranchId == user.BranchId)
        .ToListAsync());
        }
        if (fromDate == null && toDate != null)
        {
            toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);

            return View(await _context.Sales
        .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
        .Include(s => s.Customer)
        .Include(s => s.Employee)
        .Include(s => s.Branch)
        .Where(s => s.CreateDate < toDate)
        .Where(s => s.BranchId == user.BranchId)
        .ToListAsync());
        }
        if (fromDate != null && toDate != null)
        {
            toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
            
            return View(await _context.Sales
        .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
        .Include(s => s.Customer)
        .Include(s => s.Employee)
        .Include(s => s.Branch)
        .Where(s => s.CreateDate > fromDate && s.CreateDate < toDate 
        && s.BranchId == user.BranchId).ToListAsync());
        }
           
        
        return View(await _context.Sales
            .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.Branch)
            .Where(s => s.CreateDate > DateTime.Today)
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
        List<Order> orders = _context.Orders.Where(o => o.BranchId == user.BranchId)
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
        AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null) return NotFound();
        if (id == null || id == 0) return NotFound();

        Product? product = await _context.Products
            .Include(p => p.Color)
            .Include(p => p.Brand)
            .Include(p => p.ProductCategory)
            .Include(p=>p.ProductModel)
            .Where(p => p.BranchId == user.BranchId)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();

        List<Order> orders = await _context.Orders.Where(o => o.BranchId == user.BranchId)
            .ToListAsync();

        if (orders.Any(o => o.ProdId == product.Id))
        {
            return NotFound();
        }

        Order order = new();
        order.Name = product.ProductModel.ModelName;
        order.Series = product.Series;
        order.Price = product.ProductModel.ModelPrice;
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
        List<Order> orders = await _context.Orders.Where(o => o.BranchId == user.BranchId).ToListAsync();
        SaleVM saleVM = new SaleVM();
        saleVM.Orders = orders;
        ViewBag.Sellers = new SelectList(await _context.Employees
            .Where(e => e.BranchId == user.BranchId).ToListAsync(), "Id", "FullName");
        return View(saleVM);
    }
    [HttpPost]
    public async Task<IActionResult> Orders(SaleVM saleVM)
    {
        if (!User.Identity.IsAuthenticated) return BadRequest();
        AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null) return RedirectToAction("Login","Account");
        ViewBag.Sellers = new SelectList(await _context.Employees
            .Where(e => e.BranchId == user.BranchId).ToListAsync(), "Id", "FullName");
        List<Order> orders = await _context.Orders.Where(o=>o.BranchId == user.BranchId).ToListAsync();
        if (orders == null || orders.Count < 1) 
        {
            ModelState.AddModelError("Orders","Mehsul elave olunmayib !");
            saleVM.Orders = new();
            return View(saleVM); 
        }
        
        if (!ModelState.IsValid)
        {
            saleVM.Orders = orders;
            return View(saleVM);
        }
        if (orders.Sum(o => o.Price) < saleVM.CashlessPayment)
        {
            ModelState.AddModelError("CashlessPayment", "Nagdsiz odenis umumi meblegden cox ola bilmez !");
            saleVM.Orders = orders;
            return View(saleVM);
        }
        if (saleVM.CashlessPayment < 0)
        {
            ModelState.AddModelError("CashlessPayment", "Nagdsiz odenis menfi ola bilmez !");
            saleVM.Orders = orders;
            return View(saleVM);
        }
        if (saleVM.Discount < 0)
        {
            ModelState.AddModelError("Discount", "Endirim menfi ola bilmez !");
            saleVM.Orders = orders;
            return View(saleVM);
        }
        if (saleVM.Discount > 500)
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
        if (customer == null)
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
        List<Product> products = _context.Products
            .Include(p=>p.ProductModel).Include(p=>p.ProductCategory)
            .Where(p => prodId.Contains(p.Id)).ToList();
        List<SaleProducts> saleProductsList = new List<SaleProducts>();
        Salary salary = new();
       
        foreach (Product product in products)
        {
            salary.Bonus =+ product.ProductCategory.Bonus;
            SaleProducts saleProducts = new();
            saleProducts.ProductId = product.Id;
            saleProductsList.Add(saleProducts);
        }

        Sale newSale = new();
        newSale.CustomerId = customer.Id;
        newSale.SaleProducts = saleProductsList;
        newSale.TotalPrice = products.Sum(p => p.ProductModel.ModelPrice) - saleVM.Discount;
        newSale.Discount = saleVM.Discount;
        newSale.CashlessPayment = saleVM.CashlessPayment;
        newSale.TotalProfit = (products.Sum(p => p.ProductModel.ModelPrice) - saleVM.Discount) - products.Sum(p => p.CostPrice);
        newSale.EmployeeId = saleVM.EmployeeId;
        newSale.CreateDate = DateTime.Now;
        newSale.BranchId = user.BranchId;
        customer.TotalCost += products.Sum(p => p.ProductModel.ModelPrice) - saleVM.Discount;
        salary.CreateDate = DateTime.Now;
        salary.EmployeeId = saleVM.EmployeeId;
        
        


        foreach (var item in products)
        {
            item.IsSold = true;
        }
        _context.Salaries.Add(salary);
        _context.Sales.Add(newSale);
        _context.Orders.RemoveRange(orders);
        await _context.SaveChangesAsync();
        TempData["Success"] = "ok";

        return RedirectToAction("index", "home");
    }
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || id == 0) return NotFound();
        Sale? sale = await _context.Sales
            .Include(s=>s.SaleProducts).ThenInclude(sp=>sp.Product).ThenInclude(p=>p.Brand)
            .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product).ThenInclude(p => p.Color)
            .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product).ThenInclude(p => p.ProductCategory)
            .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product).ThenInclude(p => p.ProductModel)
            .Include(s=>s.Branch)
            .Include(s=>s.Customer)
            .Include(s=>s.Employee)
            .Include(s=>s.Refunds).ThenInclude(r=>r.Product).ThenInclude(p=>p.ProductModel)
            .FirstOrDefaultAsync(s=>s.Id==id);
        if(sale == null) return NotFound();
        return View(sale);
    }

}


