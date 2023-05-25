using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using ShopManagmentSystem.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using ShopManagmentSystem.ViewModels.AccountVMs;
using ShopManagmentSystem.DAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ShopManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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


        public async Task<IActionResult> Index()
        {
            List<AppUser> users = _userManager.Users.ToList();
            List<Branch> branches =await _context.Branches.ToListAsync();
            List<AccountIndexVM> accountVMs = new List<AccountIndexVM>();

            foreach (var item in users)
            {
                AccountIndexVM indexVM = new()
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    BranchName = branches.FirstOrDefault(b=>b.Id == item.BranchId).Name
                };
                accountVMs.Add(indexVM);
            }
         

            return View(accountVMs);
        }

        public IActionResult Register()
        {
            ViewBag.Branches = new SelectList(_context.Branches.Where(b=>b.Id != 5).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegistrationVM registrationVM)
        {
            ViewBag.Branches = new SelectList(_context.Branches.ToList(), "Id", "Name");
            if (!ModelState.IsValid) return View(registrationVM);
            if(await _userManager.Users.AnyAsync(u=>u.BranchId == registrationVM.BranchId))
            {
                ModelState.AddModelError("BranchId", "Bu Branch ucun User qeydiyyatdan kecirilib !");
                return View(registrationVM);
            }
            AppUser appUser = new();
            appUser.UserName = registrationVM.UserName;
            appUser.Email = registrationVM.Email;
            appUser.BranchId = registrationVM.BranchId;
            IdentityResult result = await _userManager.CreateAsync(appUser, registrationVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registrationVM);
            }

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

            string? link = Url.Action(nameof(ConfirmEmail),
                "Account",
                new { userId = appUser.Id, token },
                Request.Scheme,
                Request.Host.ToString());

            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("qurban231293@gmail.com"));
            email.To.Add(MailboxAddress.Parse(appUser.Email));
            email.Subject = "Email Confirmation";
            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/Template/verify.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{link}}", link);
            body = body.Replace("{{Fullname}}", appUser.UserName);

            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // send email
            using SmtpClient smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("qurban231293@gmail.com", "olszimzdwkxyjwwz");
            smtp.Send(email);
            smtp.Disconnect(true);



            return RedirectToAction(nameof(VerifyEmail));

        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return NotFound();

            AppUser? user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();


            await _userManager.ConfirmEmailAsync(user, token);

            await _signInManager.SignInAsync(user, false);

            await _userManager.AddToRoleAsync(user, "Shop");

            return RedirectToAction(nameof(SuccesfulRegistered));

        }


        public IActionResult VerifyEmail()
        {
            return View();
        }

        public IActionResult SuccesfulRegistered()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPassword)
        {
            if (!ModelState.IsValid) return View();

            AppUser? exsistUser = await _userManager.FindByEmailAsync(forgotPassword.Email);

            if (exsistUser == null)
            {
                ModelState.AddModelError("Email", "User not Found");
                return View();
            }


            string token = await _userManager.GeneratePasswordResetTokenAsync(exsistUser);

            string? link = Url.Action(nameof(ResetPassword),
                "Account", new { userId = exsistUser.Id, token },
                 Request.Scheme, Request.Host.ToString());



            // create email message
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("qurban231293@gmail.com"));
            email.To.Add(MailboxAddress.Parse(exsistUser.Email));
            email.Subject = "Verify password reset email";
            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/Template/Verify.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{link}}", link);
            body = body.Replace("{{Fullname}}", exsistUser.UserName);

            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // send email
            using SmtpClient smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("qurban231293@gmail.com", "olszimzdwkxyjwwz");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction(nameof(VerifyEmail));
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {

            AppUser? exsistUser = await _userManager.FindByIdAsync(userId);

            bool checkToken = await _userManager.VerifyUserTokenAsync(exsistUser,
                _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);

            if (!checkToken) return NotFound();

            ResetPasswordVM resetPasswordVM = new ResetPasswordVM()
            {
                UserId = userId,
                Token = token
            };

            return View(resetPasswordVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword)
        {
            if (!ModelState.IsValid) return View(resetPassword);

            AppUser? exsistUser = await _userManager.FindByIdAsync(resetPassword.UserId);

            if (exsistUser == null) return NotFound();

            if (await _userManager.CheckPasswordAsync(exsistUser, resetPassword.Password))
            {
                ModelState.AddModelError("", "This Password is your old password");
                return View(resetPassword);
            }

            await _userManager.ResetPasswordAsync(exsistUser, resetPassword.Token, resetPassword.Password);
            await _userManager.UpdateSecurityStampAsync(exsistUser);

            return RedirectToAction("login","Account");
        }
    }
}
