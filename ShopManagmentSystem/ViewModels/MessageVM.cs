using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.ViewModels
{
    public class MessageVM
    {
        public List<Message> Messages { get; set; }
        public AppUser User { get; set; }
    }
}
