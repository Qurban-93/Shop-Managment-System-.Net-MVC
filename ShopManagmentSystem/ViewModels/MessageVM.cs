using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.ViewModels
{
    public class MessageVM
    {
        public List<Message> Messages { get; set; }
        public AppUser User { get; set; }
        public int CountSkip { get; set; }
        public List<int> UnreadIds { get; set; }
        public string LastSeen { get; set; }
    }
}
