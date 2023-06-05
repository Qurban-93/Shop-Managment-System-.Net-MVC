using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels.AccountVMs;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Enums;

namespace ShopManagmentSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

      

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string? ReturnUrl)
        {
            if(User.Identity.IsAuthenticated) return RedirectToAction("index","home");
            if (loginVM == null) return NotFound();
            if (!ModelState.IsValid) return View();
            AppUser? user = await _userManager.FindByNameAsync(loginVM.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or Email or Password invalid!");
                return View();
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync
                (user, loginVM.Password, loginVM.RememmberMe, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Account bloked !");
                return View(loginVM);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Email or Password invalid!");
                return View(loginVM);
            }

            await _signInManager.SignInAsync(user, true);

            if (ReturnUrl != null) return Redirect(ReturnUrl); ;

            return RedirectToAction("Index", "Sale");
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(RoleEnum)))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
            }
            return Content("elave edildi");
        }

    }
}
