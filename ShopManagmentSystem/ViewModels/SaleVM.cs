using ShopManagmentSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels
{
    public class SaleVM
    {
        public List<ProductVM> Products { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public string Customer { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public string? CustomerEmail { get; set; }
        public double PayWithCard { get; set; }

    }
}
