using Microsoft.Identity.Client;
using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Employee : BaseEntity
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Number { get; set; }
        public int EmployeePostionId { get; set; }
        public EmployeePosition? EmployeePostion { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public List<Sale>? Sales { get; set; }
        public List<Refund>? Refunds { get; set; }
        public List<Salary>? Salaries { get; set; }
        public bool IsDeleted { get; set; }
    }
}
