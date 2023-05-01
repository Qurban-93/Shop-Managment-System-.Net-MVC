using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels.AccountVMs
{
    public class ForgotPasswordVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
