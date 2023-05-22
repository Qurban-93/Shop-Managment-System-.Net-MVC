using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Salary : BaseEntity
    {
        public double Bonus { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int? SaleId { get; set; }
        public Sale? Sale { get; set; }
        public int? RefundId { get; set; }
        public Refund? Refund { get; set; }
        public int? PunishmentId { get; set; }
        public Punishment? Punishment { get; set; }
    }
}
