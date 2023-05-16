using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Customer : BaseEntity
    {
        public string FullName { get; set; }
        public string? Email { get; set; }       
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public double? TotalCost { get; set; }
        public List<Sale>? Sales { get; set; }
        public List<Refund>? Refunds { get; set; }

    }
}
