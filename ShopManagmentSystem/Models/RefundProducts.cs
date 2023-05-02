using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class RefundProducts
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int RefundId { get; set; }
        public Refund? Refund { get; set; }
    }
}
