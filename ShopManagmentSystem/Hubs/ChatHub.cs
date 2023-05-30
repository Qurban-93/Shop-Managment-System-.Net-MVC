using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;

        public ChatHub(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task<Task> OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser? user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.ConnectionId = Context.ConnectionId;
                user.LastSeen = "online";
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
                user.ConnectionId = null;
                user.LastSeen = DateTime.Now.ToString("dd MMMM yyyy , hh:mm");
                var result = _userManager.UpdateAsync(user).Result;
                await Clients.All.SendAsync("Offline", user.Id);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}

