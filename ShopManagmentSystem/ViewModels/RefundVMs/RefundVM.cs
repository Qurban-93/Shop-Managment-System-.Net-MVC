using ShopManagmentSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels.RefundVMs
{
    public class RefundVM
    {
        [Required]
        public int RefundOrderId { get; set; }
        public RefundOrder? Order { get; set; }
        public double CashlessPayment { get; set; }
        public double Discount { get; set; }
    }
}
