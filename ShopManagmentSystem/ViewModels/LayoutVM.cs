using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.ViewModels
{
    public class LayoutVM
    {
        public int CountOrder { get; set; }
        public IList<string> Role { get; set; }
        public int MessageCount { get; set; }
        public string UserId { get; set; }
    }
}
