using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels.AccountVMs;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Enums;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;


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

            return RedirectToAction("login", "Account");
        }

    }
}
