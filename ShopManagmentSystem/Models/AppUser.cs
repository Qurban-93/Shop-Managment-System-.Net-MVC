using Microsoft.AspNetCore.Identity;

namespace ShopManagmentSystem.Models
{
    public class AppUser: IdentityUser
    {
        public string FullName { get; set; }
    }
}
