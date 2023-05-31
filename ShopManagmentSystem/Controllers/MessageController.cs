using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Hubs;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

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
            
            ViewBag.Users = _userManager.Users.Where(u=>u.UserName != User.Identity.Name).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChatHistory(string Id)
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (Id == null || user == null) return NotFound();
            List<Message> messages = _context.Messages.Where(m => (m.DestinationId == Id && m.SenderId == user.Id) ||
            (m.DestinationId == user.Id && m.SenderId == Id)).ToList();
            MessageVM messageVM = new()
            {
                Messages= messages,
                User = user,
            };
            return PartialView("_ChatHistoryPartialView",messageVM);
        }
    }
}
