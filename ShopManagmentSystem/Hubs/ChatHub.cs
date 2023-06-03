using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IHubContext<UpdateHub> _updateHub;

        public ChatHub(UserManager<AppUser> userManager, AppDbContext context, IHubContext<UpdateHub> updateHub)
        {
            _userManager = userManager;
            _context = context;
            _updateHub = updateHub;
        }

        public override async Task<Task> OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser? user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.IsOnline = true;
                user.LastSeen = "online";
                user.ConnectionId = Context.ConnectionId;
                var result = _userManager.UpdateAsync(user).Result;
                await Clients.All.SendAsync("Online", user.Id);
            }
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser? user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.IsOnline = false;
                user.LastSeen = DateTime.Now.ToString("dd MMMM yyyy , hh:mm tt");
                user.ConnectionId = null;
                var result = _userManager.UpdateAsync(user).Result;
                await Clients.All.SendAsync("Offline", user.Id);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message, string senderUserId, string destinationUserId)
        {
            Message newMessage = new();
            AppUser? destinationUser = await _userManager.FindByIdAsync(destinationUserId);
            AppUser? senderUser = await _userManager.FindByIdAsync(senderUserId);
            if (destinationUser != null && destinationUser.ConnectionId != null)
            {
                
                await Clients.Client(destinationUser.ConnectionId).SendAsync("ShowMessage", senderUserId, message, destinationUserId);
                newMessage.IsRead = true;
            }
            else
            {
                _updateHub.Clients.All.SendAsync("NewMessageCount",destinationUserId);
            }         

            if (!string.IsNullOrWhiteSpace(message) && destinationUser != null && senderUser != null &&
                destinationUser != null && senderUser != null)
            {
                newMessage.SenderId = senderUserId;
                newMessage.DestinationId = destinationUserId;
                newMessage.Content = message;
                newMessage.CreateDate = DateTime.Now;

                _context.Messages.Add(newMessage);
                await _context.SaveChangesAsync();
            }
        }
    }
}

