using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Money : BaseEntity
    {
        public double Incoming { get; set; }
        public double CashlessPayment { get; set; }
        public double Discount { get; set; }
        public int? SaleId { get; set; }
        public Sale? Sale { get; set; }
        public int? RefundId { get; set; }
        public Refund? Refund { get; set; }
        public int? ExpensesId { get; set; }
        public Punishment? Expenses { get; set; }
        public int BranchId { get; set; }

    }
}
