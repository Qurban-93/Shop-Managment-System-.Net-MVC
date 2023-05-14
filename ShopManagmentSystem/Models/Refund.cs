using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Refund : BaseEntity
    {
        public double TotalPrice { get; set; }
        public double CashlessPayment { get; set; }
        public double Discount { get; set; }
        public double TotalLoss { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int? SaleId { get; set; }
        public Sale? Sale { get; set; }
        public string? Description { get; set; }
    }
}
