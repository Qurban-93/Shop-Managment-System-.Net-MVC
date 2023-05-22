using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels
{
    public class PunishmentCreateVM
    {
        [Required]
        public double Amount { get; set; }
        [Required]
        public string? Descpription { get; set; }
        [Required]
        public int EmployeeId { get; set; }
    }
}
