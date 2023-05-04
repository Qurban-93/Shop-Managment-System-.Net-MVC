using ShopManagmentSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels
{
    public class RefundVM
    {
        public List<RefundOrder>? Orders { get; set; }
        [Required]    
        public double CashlessPayment { get; set; }
        public double Discount { get; set; }
    }
}
