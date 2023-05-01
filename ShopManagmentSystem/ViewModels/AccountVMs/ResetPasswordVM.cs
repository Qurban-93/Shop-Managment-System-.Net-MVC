using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels.AccountVMs
{
    public class ResetPasswordVM
    {
        [Required,MinLength(8, ErrorMessage = "Parolun uzunlugu 8 simvoldan ibaret olmalidi !")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required,MinLength(8,ErrorMessage ="Parolun uzunlugu 8 simvoldan ibaret olmalidi !")]
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
        public string? Token { get; set; }
        public string? UserId { get; set; }
    }
}
