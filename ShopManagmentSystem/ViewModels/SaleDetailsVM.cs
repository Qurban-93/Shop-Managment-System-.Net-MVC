using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.ViewModels
{
    public class SaleDetailsVM
    {
        public Sale? Sale { get; set; }
        public List<RefundOrder>? RefundOrders { get; set; }
    }
}
