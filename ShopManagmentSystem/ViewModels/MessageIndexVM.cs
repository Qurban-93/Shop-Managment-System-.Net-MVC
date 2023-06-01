using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.ViewModels
{
    public class MessageIndexVM
    {
        public List<AppUser> Users { get; set; }
        public List<Message> NewMessages { get; set; }
    }
}
