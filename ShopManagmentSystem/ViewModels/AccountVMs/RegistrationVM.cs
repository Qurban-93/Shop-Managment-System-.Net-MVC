using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels.AccountVMs
{
    public class RegistrationVM
    {
        
        [Required, StringLength(100)]
        public string? UserName { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required, DataType(DataType.Password),Compare(nameof(Password))]
        public string? RepeatPassword { get; set; }
        [Required]
        public int BranchId { get; set; }

    }
}
