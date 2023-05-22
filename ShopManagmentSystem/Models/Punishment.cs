using ShopManagmentSystem.Models.Base;
using ShopManagmentSystem.ViewModels.EmployeeVMs;

namespace ShopManagmentSystem.Models
{
    public class Punishment : BaseEntity
    {
        public double Amount { get; set; }
        public string? Descpription { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

    }
}
