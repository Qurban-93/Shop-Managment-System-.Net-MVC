using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Hubs;
using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.Controllers
{
    public class MessageController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly AppDbContext _context;

        public MessageController(UserManager<AppUser> userManager, IHubContext<ChatHub> hubContext, AppDbContext context)
        {
            _userManager = userManager;
            _hubContext = hubContext;
            _context = context;
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

        public async Task<IActionResult> ChatHistory(string destinationId)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (destinationId == null || user == null) return NotFound();
            List<Message> messages = _context.Messages.Where(m => m.DestinationId == destinationId && m.SenderId == user.Id).ToList();
            return PartialView("__ChatHistoryPartialView",messages);
        }
    }
}
