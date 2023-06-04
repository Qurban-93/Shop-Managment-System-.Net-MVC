using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class RefundOrder : BaseEntity
    {
       
        public int ProdId { get; set; }
        public int SaleId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Series { get; set; }
        public double Price { get; set; }
        public string Category { get; set;}
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string EmployeeName { get; set; }
        public int? BranchId { get; set; }
        public string? Description { get; set; }

    }
}
