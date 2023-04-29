using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels
{
    public class CustomerCreateVM
    {
        [Required]
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
    }
}
