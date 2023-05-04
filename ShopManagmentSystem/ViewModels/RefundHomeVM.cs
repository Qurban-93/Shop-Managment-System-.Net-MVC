using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.ViewModels
{
    public class RefundHomeVM
    {
        public List<RefundOrder> RefundOrders { get; set; }
        public List<Refund> Refunds { get; set; }
    }
}
