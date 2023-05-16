using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class EmployeePosition : BaseEntity
    {
        public string PositionName { get; set; }
        public double? FixSalary { get; set; }
    }
}
