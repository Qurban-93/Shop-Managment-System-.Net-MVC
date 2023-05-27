using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ShopManagmentSystem.Hubs;
using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.Controllers
{
    public class MessageController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(UserManager<AppUser> userManager, IHubContext<ChatHub> hubContext)
        {
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            ViewBag.Users = _userManager.Users.ToList();
            return View();
        }

        public async Task<IActionResult> ShowAlert(string id)
        {
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user.ConnectionId != null)
                _hubContext.Clients.Client(user.ConnectionId).SendAsync("ShowAlert", User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }
    }
}
