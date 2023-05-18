using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels.EmployeeVMs
{
    public class EmployeePositionCreateVM
    {
        [Required]
        public double FixSalary { get; set; }
        [Required]
        public string? PositionName { get; set; }
    }
}
