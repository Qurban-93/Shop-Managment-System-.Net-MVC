using Microsoft.AspNetCore.Identity;

namespace ShopManagmentSystem.Models
{
    public class AppUser: IdentityUser
    {   
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
        public string? ConnectionId { get; set; }
        public bool IsOnline { get; set; }
        public string LastSeen { get; set; }

    }
}
