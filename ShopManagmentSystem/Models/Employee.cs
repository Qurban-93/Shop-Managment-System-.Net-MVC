using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Employee : BaseEntity
    {
        public string FullName { get; set; }
        public byte Age { get; set; }
        public string Number { get; set; }
        public int EmployeePositionId { get; set; }
        public EmployeePostion EmployeePostion { get; set; }
        public int? BranchId { get; set; }
    }
}
