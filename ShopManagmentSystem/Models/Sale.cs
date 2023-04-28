using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Sale : BaseEntity
    {
        public double TotalPrice { get; set; }
        public double CashlessPayment { get; set; }
        public double Discount { get; set; }
        public double TotalProfit { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public List<Product> Products { get; set; }
    }
}
