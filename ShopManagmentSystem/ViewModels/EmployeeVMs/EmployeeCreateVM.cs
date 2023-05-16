using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels.EmployeeVMs
{
    public class EmployeeCreateVM
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public int EmployeePositionId { get; set; }
        [Required]
        public int BranchId { get; set; }
    }
}
