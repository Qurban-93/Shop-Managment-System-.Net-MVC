using Microsoft.AspNetCore.Identity;
using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.ViewModels
{
    public class AccountEditVM
    {
        public AppUser User { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public IList<string> UserRoles { get; set; }
        public Branch Branch { get; set; }
    }
}
