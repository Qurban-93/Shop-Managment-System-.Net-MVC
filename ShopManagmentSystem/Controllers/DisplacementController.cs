using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;
using ShopManagmentSystem.ViewModels.ProductVMs;

namespace ShopManagmentSystem.Controllers
{
    public class DisplacementController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DisplacementController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Displacement
                .Include(d=>d.Products)
                .ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            ViewBag.Destination = new SelectList(await _context.Branches
                .Where(b => b.Id != user.BranchId).ToListAsync(), "Id", "Name");
            DisplacementCreateVM createVM;
            string basket = Request.Cookies["Basket"];
            if (basket == null)
            {
                createVM = new()
                {
                    Products = await _context.Products
                .Include(p => p.Color)
                .Include(p => p.ProductModel)
                .Include(p => p.ProductCategory)
                .Include(p => p.Brand)
                .Where(p => !p.IsSold && p.BranchId == user.BranchId)
                .ToListAsync(),
                };
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
                    .Where(p => !itemsId.Contains(p.Id) && p.BranchId == user.BranchId).ToListAsync();
                ViewBag.List = JsonConvert.DeserializeObject<List<ResendProductVM>>(basket);
            }
            return View(createVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(DisplacementCreateVM createVM)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            ViewBag.Destination = new SelectList(await _context.Branches
                .Where(b => b.Id != user.BranchId).ToListAsync(), "Id", "Name");
            if (!ModelState.IsValid) return View();
            string basket = Request.Cookies["basket"];
            if (string.IsNullOrWhiteSpace(basket)) return BadRequest();
            List<int> itemsId = JsonConvert.DeserializeObject<List<ResendProductVM>>(basket).Select(i => i.Id).ToList();
            List<Product> products = await _context.Products.Where(p => itemsId.Contains(p.Id)).ToListAsync();
            if (products.Count == 0 || products == null) return BadRequest();
            Displacement displacement = new()
            {
                SenderId = user.BranchId,
                SenderBranch = _context.Branches.FirstOrDefault(b => b.Id == user.BranchId).Name,
                DestinationId = createVM.DestinationId,
                DestinationBranch = _context.Branches.FirstOrDefault(b => b.Id == createVM.DestinationId).Name,
                Products = products   ,
                CreateDate = DateTime.Now,
            };
            foreach (Product product in products)
            {
                product.BranchId = 5;
            }
            await _context.Displacement.AddAsync(displacement);
            await _context.SaveChangesAsync();

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
            ResendProductVM productVM = products.FirstOrDefault(p=>p.Id == id);
            if (productVM == null) return BadRequest();
            products.Remove(productVM);
            Response.Cookies.Append("Basket", JsonConvert.SerializeObject(products),
               new CookieOptions { MaxAge = TimeSpan.FromMinutes(5) });

            return Ok(productVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == 0 || id == null) return BadRequest();
            Displacement? displacement = await _context.Displacement
                .Include(d=>d.Products)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (displacement == null) return NotFound();
            return View(displacement);
        }
    }
}
