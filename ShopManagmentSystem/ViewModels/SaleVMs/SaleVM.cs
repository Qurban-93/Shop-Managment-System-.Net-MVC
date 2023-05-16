using ShopManagmentSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels.SaleVMs
{
    public class SaleVM
    {
        public List<Order>? Orders { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string? CustomerEmailOrPhoneNumber { get; set; }
        public double CashlessPayment { get; set; }
        public double Discount { get; set; }

    }
}
