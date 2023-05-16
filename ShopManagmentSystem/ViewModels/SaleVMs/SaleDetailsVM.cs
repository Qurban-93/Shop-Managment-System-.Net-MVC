using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.ViewModels.SaleVMs
{
    public class SaleDetailsVM
    {
        public Sale? Sale { get; set; }
        public List<RefundOrder>? RefundOrders { get; set; }
    }
}
