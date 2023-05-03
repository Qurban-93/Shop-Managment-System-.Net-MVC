using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class RefundOrder
    {
        public int Id { get; set; }
        public int ProdId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Series { get; set; }
        public double Price { get; set; }
        public string Category { get; set;}
        public string CustomerEmail { get; set; }
        public int? BranchId { get; set; }

    }
}
